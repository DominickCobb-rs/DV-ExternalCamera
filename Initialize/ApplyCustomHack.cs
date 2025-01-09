using UnityEngine;
using DV.TerrainSystem;
using JBooth.MicroSplat;
using System.Reflection;
using System.Collections.Generic;

internal class ApplyCustomHack : MonoBehaviour
{
    public static void Apply()
    {
        if(VRManager.IsVREnabled())
            return;
        List<GameObject> childrenToDelete = new List<GameObject>();
        Terrain[] terrains = FindObjectsOfType<Terrain>();

        if (terrains == null || terrains.Length == 0)
        {
            return;
        }
        foreach (Terrain terrain in terrains)
        {
            if (terrain.gameObject.name.ToLower().Contains("renderer"))
            {
                continue;
            }
            MicroSplatVisibilityHack existingHack = terrain.GetComponent<MicroSplatVisibilityHack>();
            if (existingHack != null)
            {
                Transform childTransform = terrain.transform.Find(terrain.name + "_renderer");
                if (childTransform != null)
                {
                    GameObject childObject = childTransform.gameObject;
                    MicroSplatTerrain childMicroSplat = childObject.GetComponent<MicroSplatTerrain>();

                    if (childMicroSplat != null)
                    {
                        MicroSplatTerrain parentMicroSplat = terrain.gameObject.AddComponent<MicroSplatTerrain>();
                        CopyComponentValues(childMicroSplat, parentMicroSplat);
                        childrenToDelete.Add(childObject);
                    }
                }
                existingHack.enabled = false;
                Destroy(existingHack);
            }
        }

        foreach (GameObject child in childrenToDelete)
        {
            Destroy(child);
        }

        terrains = FindObjectsOfType<Terrain>();
        foreach (Terrain terrain in terrains)
        {
            if (terrain.GetComponent<MicroSplatTerrain>())
                terrain.gameObject.AddComponent<CustomMicroSplatVisibilityHack>();
            else
                Debug.LogError($"No MicroSplatTerrain attached to {terrain.gameObject.name}");
        }
    }

    // Previously used the one in Main
    // Previously used to copy the old _renderer MicroSplatTerrain to a temporary one that is then applied to the new object

    public static void CopyComponentValues(Component original, Component copy)
    {
        System.Type type = original.GetType();
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default;

        // Copy all fields
        FieldInfo[] fields = type.GetFields(flags);
        foreach (FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }

        // Copy all properties
        PropertyInfo[] properties = type.GetProperties(flags);
        foreach (PropertyInfo property in properties)
        {
            if (property.CanWrite && property.CanRead)
            {
                property.SetValue(copy, property.GetValue(original, null), null);
            }
        }
    }
}
