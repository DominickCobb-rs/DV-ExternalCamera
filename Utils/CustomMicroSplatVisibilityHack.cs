using System;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using JBooth.MicroSplat;
using UnityEngine;

namespace DV.TerrainSystem
{
    public class CustomMicroSplatVisibilityHack : MonoBehaviour
    {
        private static Material _defaultMat;
        private static Plane[] customCameraPlanes = new Plane[6];
        private static Plane[] leftEyePlanes = new Plane[6];
        private static Plane[] rightEyePlanes = new Plane[6];
        private static int frameCount;

        private MicroSplatTerrain childMicroSplat;
        private Terrain ogTerrain;
        private Terrain childTerrain;
        private bool wasVisible;

        private Camera customCamera = CustomCameraUtils.newCameraObject.GetComponent<Camera>();

        private static Material DefaultMat
        {
            get
            {
                if (!_defaultMat)
                {
                    _defaultMat = new Material(Shader.Find("Nature/Terrain/Diffuse"));
                }

                return _defaultMat;
            }
        }

        private static void CheckEyePlanes()
        {
            if (frameCount != Time.frameCount)
            {
                frameCount = Time.frameCount;
                Camera main = Camera.main;
                if (main.stereoEnabled)
                {
                    GeometryUtility.CalculateFrustumPlanes(main.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left) * main.GetStereoViewMatrix(Camera.StereoscopicEye.Left), leftEyePlanes);
                    GeometryUtility.CalculateFrustumPlanes(main.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right) * main.GetStereoViewMatrix(Camera.StereoscopicEye.Right), rightEyePlanes);
                }
                else
                {
                    GeometryUtility.CalculateFrustumPlanes(main, leftEyePlanes);
                }
            }
        }

        private static void CheckCameraPlanes(Camera camera, Plane[] planes)
        {
            if (frameCount != Time.frameCount)
            {
                frameCount = Time.frameCount;
                if (camera.stereoEnabled)
                {
                    GeometryUtility.CalculateFrustumPlanes(camera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left) * camera.GetStereoViewMatrix(Camera.StereoscopicEye.Left), leftEyePlanes);
                    GeometryUtility.CalculateFrustumPlanes(camera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right) * camera.GetStereoViewMatrix(Camera.StereoscopicEye.Right), rightEyePlanes);
                }
                else
                {
                    GeometryUtility.CalculateFrustumPlanes(camera, planes);
                }
            }
        }

        private void Awake()
        {
            ogTerrain = GetComponent<Terrain>();
            ogTerrain.drawHeightmap = false;
            ogTerrain.materialTemplate = null;
            GameObject gameObject = new GameObject(base.name + "_renderer");
            gameObject.layer = ogTerrain.gameObject.layer;
            gameObject.transform.parent = base.transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            childTerrain = gameObject.AddComponent<Terrain>();
            MicroSplatTerrain component = GetComponent<MicroSplatTerrain>();
            component.keywordSO.DisableKeyword("_CUSTOMSPLATTEXTURES");
            childMicroSplat = gameObject.AddComponent<MicroSplatTerrain>();
            childMicroSplat.templateMaterial = component.templateMaterial;
            childMicroSplat.propData = component.propData;
            childMicroSplat.keywordSO = component.keywordSO;
            childMicroSplat.useCustomTexturesWithoutKeyword = true;
            childMicroSplat.OnMaterialSync += OnMatSync;
            Destroy(component);
            OnTerrainDataLoaded(ogTerrain.terrainData, default(Vector2Int));
            TerrainGrid.TerrainDataLoaded += OnTerrainDataLoaded;
        }

        private void Start()
        {
            childTerrain.groupingID = ogTerrain.groupingID;
            childTerrain.allowAutoConnect = ogTerrain.allowAutoConnect;
            childTerrain.heightmapPixelError = ogTerrain.heightmapPixelError;
            childTerrain.heightmapMaximumLOD = ogTerrain.heightmapMaximumLOD;
            childTerrain.basemapDistance = ogTerrain.basemapDistance;
            childTerrain.drawInstanced = ogTerrain.drawInstanced;
            childTerrain.drawTreesAndFoliage = false;
        }

        private void OnMatSync(Material m)
        {
            if (!IsVisible())
            {
                Toggle(on: false);
            }
        }

        private void OnDestroy()
        {
            TerrainGrid.TerrainDataLoaded -= OnTerrainDataLoaded;
        }

        public void SetCustomCamera(Camera camera)
        {
            customCamera = camera;
        }

        private void OnTerrainDataLoaded(TerrainData terraindata, Vector2Int _)
        {
            if (terraindata == ogTerrain.terrainData)
            {
                if ((bool)childTerrain.terrainData)
                {
                    UnityEngine.Object.Destroy(childTerrain.terrainData);
                }

                childTerrain.terrainData = UnityEngine.Object.Instantiate(terraindata);
                childTerrain.terrainData.terrainLayers = Array.Empty<TerrainLayer>();
                Texture2D[] alphamapTextures = ogTerrain.terrainData.alphamapTextures;
                childMicroSplat.customControl0 = ((alphamapTextures.Length != 0) ? alphamapTextures[0] : Texture2D.blackTexture);
                childMicroSplat.customControl1 = ((alphamapTextures.Length > 1) ? alphamapTextures[1] : Texture2D.blackTexture);
                childMicroSplat.customControl2 = ((alphamapTextures.Length > 2) ? alphamapTextures[2] : Texture2D.blackTexture);
                childMicroSplat.customControl3 = ((alphamapTextures.Length > 3) ? alphamapTextures[3] : Texture2D.blackTexture);
                childMicroSplat.customControl4 = ((alphamapTextures.Length > 4) ? alphamapTextures[4] : Texture2D.blackTexture);
                childMicroSplat.customControl5 = ((alphamapTextures.Length > 5) ? alphamapTextures[5] : Texture2D.blackTexture);
                childMicroSplat.customControl6 = ((alphamapTextures.Length > 6) ? alphamapTextures[6] : Texture2D.blackTexture);
                childMicroSplat.customControl7 = ((alphamapTextures.Length > 7) ? alphamapTextures[7] : Texture2D.blackTexture);
                childMicroSplat.Sync();
                ogTerrain.enabled = false;
            }
        }

        private void LateUpdate()
        {
            bool flag = IsVisible();
            if (flag != wasVisible)
            {
                Toggle(flag);
            }
        }

        private bool IsVisible()
        {
            Camera main = Camera.main;
            if (!main || !ogTerrain || !ogTerrain.terrainData)
            {
                return false;
            }

            Bounds bounds = GetBounds();
            CheckEyePlanes();
            if (main.stereoEnabled)
            {
                if (!GeometryUtility.TestPlanesAABB(leftEyePlanes, bounds))
                {
                    return GeometryUtility.TestPlanesAABB(rightEyePlanes, bounds);
                }
            }

            if (customCamera != null)
            {
                CheckCameraPlanes(customCamera, customCameraPlanes);
                return GeometryUtility.TestPlanesAABB(leftEyePlanes, bounds) || GeometryUtility.TestPlanesAABB(customCameraPlanes, bounds);
            }

            return GeometryUtility.TestPlanesAABB(leftEyePlanes, bounds);
        }

        private Bounds GetBounds()
        {
            TerrainData terrainData = ogTerrain.terrainData;
            if (terrainData == null)
            {
                Debug.Log($"TerrainData null for {ogTerrain.name} on {ogTerrain.transform.parent.name}");
            }
            Bounds result = default(Bounds);
            if (!transform)
            {
                Debug.LogError($"Transform null for {name}");
            }
            if (transform.position == null)
            {
                Debug.LogError($"Transform position null for {name}");
            }
            result.center = transform.position + terrainData.size * 0.5f;
            result.size = terrainData.size;
            return result;
        }

        private void Toggle(bool on)
        {
            wasVisible = on;
            childTerrain.materialTemplate = on ? childMicroSplat.matInstance : DefaultMat;
        }
    }
}
