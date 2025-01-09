using UnityEngine;
using DV.TerrainSystem;
using DvMod.CustomCamera;

using System.Collections;

internal class Initializer : MonoBehaviour
{
    private static Coroutine waitForLoad;
    private static GameObject initializerObject;
    private static TerrainGrid terrainGrid;
    static IEnumerator WaitForLoad()
    {
        do
        {
            terrainGrid = FindObjectOfType<TerrainGrid>();
            yield return null;
        }
        while (terrainGrid == null || terrainGrid.IsLoadingInProgress() || !Camera.main);
        Debug.Log("Camera.main found, creating camera");
        CustomCameraUtils.CreateCamera();
        Debug.Log("Applying custom hack to terrain");
        ApplyCustomHack.Apply();
        Destroy(initializerObject);
        yield break;
    }
    public static void Initialize()
    {
        terrainGrid = FindObjectOfType<TerrainGrid>();
        GameObject initializerObject = new GameObject("Initializer");
        Initializer initializer = initializerObject.AddComponent<Initializer>();
        waitForLoad = initializer.StartCoroutine(WaitForLoad());
        Main.Unsubscribe();
    }
}
