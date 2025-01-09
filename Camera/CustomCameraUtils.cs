using System.Reflection;

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using VRTK;

using AwesomeTechnologies.VegetationSystem;
using PlaceholderSoftware.WetStuff;
using DV.VFX;

public static class CustomCameraUtils
{
    public static GameObject newCameraObject;
    private static TrainCar previousTrainCar;
    public static Camera customCamera;
    public static bool camIsInCab = false;

    // VR BREAKS EVERYTHING
    public static void CreateCamera()
    {
        if (newCameraObject != null)
            return;
        newCameraObject = new GameObject("CustomCameraObject");
        int captureResolutionWidth = 1920, captureResolutionHeight = 1080;
        newCameraObject.SetActive(false);
        newCameraObject.name = "CustomCamera";
        newCameraObject.tag = "Untagged";
        customCamera = newCameraObject.AddComponent<Camera>();
        customCamera.enabled = false;
        AllocateNecessaryComponents(newCameraObject);
        RenderTextureDescriptor rtDesc = new RenderTextureDescriptor(captureResolutionWidth, captureResolutionHeight)
        {
            depthBufferBits = 24,
            graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_SRGB, // Default color format
            dimension = UnityEngine.Rendering.TextureDimension.Tex2D,
            msaaSamples = 1,
            volumeDepth = 1,
            sRGB = true
        };

        RenderTexture renderTexture = new RenderTexture(rtDesc);
        customCamera.targetTexture = renderTexture;

        UnityCapture unityCapture = newCameraObject.AddComponent<UnityCapture>();

        unityCapture.CaptureDevice = UnityCapture.ECaptureDevice.CaptureDevice1;
        unityCapture.ResizeMode = UnityCapture.EResizeMode.Disabled;
        unityCapture.Timeout = 1000;
        unityCapture.MirrorMode = UnityCapture.EMirrorMode.Disabled;
        unityCapture.DoubleBuffering = true;
        unityCapture.EnableVSync = true;
        unityCapture.TargetFrameRate = 30;
        unityCapture.HideWarnings = false;

        unityCapture.GetComponent<Camera>().targetTexture = renderTexture;
        VegetationSystemPro vegetationSystemPro = Object.FindObjectOfType<VegetationSystemPro>();
        vegetationSystemPro?.AddCamera(customCamera, noFrustumCulling: false, renderDirectToCamera: true);
        newCameraObject.AddComponent<FPSLimiter>();
    }

    public static void AttachCamera(TrainCar trainCar, Vector3 position, Quaternion rotation, bool isInCab = false)
    {
        if (isInCab)
        {
            if (newCameraObject.GetComponent<LODEnforcer>() != null)
            {
                Object.Destroy(newCameraObject.GetComponent<LODEnforcer>());
            }
            newCameraObject.AddComponent<LODEnforcer>();
            TrainPhysicsLod trainPhysicsLod = trainCar.GetComponent<TrainPhysicsLod>();
            trainPhysicsLod.LockHighestLOD();
            if (!trainCar.IsInteriorLoaded)
                trainCar.LoadInterior();
        }
        else if (previousTrainCar != null)
        {
            ClearPreviousTrainLOD();
        }
        newCameraObject.transform.SetParent(trainCar.transform);
        newCameraObject.transform.localPosition = position;
        newCameraObject.transform.localRotation = rotation;
        previousTrainCar = trainCar;
        newCameraObject.SetActive(true);
        SetTheRenderingPath();
    }

    public static void CopyComponents(GameObject source, GameObject destination)
    {
        Component[] components = source.GetComponents<Component>();
        foreach (Component component in components)
        {
            if (component is Transform ||
                component is Camera ||
                component is AudioListener ||
                component is CameraZoom ||
                component is ShadowTracer ||
                component is SteamVRFrustumAdjust ||
                component is SteamVR_Camera ||
                component is SteamVR_Fade ||
                component is VRTK_TrackedHeadset)
                continue;
            Component newComponent = destination.AddComponent(component.GetType());
            CopyComponentValues(component, newComponent);
        }
    }

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

    public static void ClearCamera()
    {
        if (newCameraObject != null)
        {
            newCameraObject.SetActive(false);
            ClearPreviousTrainLOD();
        }
    }

    private static void ClearPreviousTrainLOD()
    {
        TrainPhysicsLod trainPhysicsLod = previousTrainCar.GetComponent<TrainPhysicsLod>();
        trainPhysicsLod?.UnlockHighestLOD();
    }

    private static void SetTheRenderingPath()
    {
        // This has been problematic. The camera is very finicky about where you set the shading path
        // Spent at least an hour trying to figure out where you have to specify the shading path
        // in the order of enabling objects
        Debug.Log("SETTING RENDERING PATH");
        customCamera.renderingPath = RenderingPath.DeferredShading;
        Debug.Log($"Actual rendering path: {customCamera.actualRenderingPath}");
    }

    private static void AllocateNecessaryComponents(GameObject gameObject)
    {
        // Match the nonVRCamera settings
        customCamera.allowHDR = true;
        customCamera.allowMSAA = false;
        customCamera.nearClipPlane = .1f;
        customCamera.farClipPlane = 22000;
        customCamera.fieldOfView = 50;
        customCamera.cullingMask = 267386679;
        customCamera.layerCullSpherical = true;
        customCamera.forceIntoRenderTexture = true;

        gameObject.AddComponent<FlareLayer>();
        // PostProcessLayer being active on the newly created camera destroys everything
        if (!VRManager.IsVREnabled())
        {
            PostProcessLayer existingPostProcessLayer = Object.FindObjectOfType<PostProcessLayer>();
            PostProcessLayer postProcessLayer = gameObject.AddComponent<PostProcessLayer>();
            if (existingPostProcessLayer)
            {
                CopyComponentValues(existingPostProcessLayer, postProcessLayer);
                Debug.Log("Copied existing PostProcessLayer settings!");
            }
            else
            {
                postProcessLayer.volumeTrigger = gameObject.transform;
                postProcessLayer.volumeLayer = 33554960; // Found on default nonVRcamera
                Debug.Log("Created new PostProcessLayer!");
            }
        }
        gameObject.AddComponent<TOD_Camera>();
        gameObject.AddComponent<TOD_UnderwaterLevelSetter>();
        gameObject.AddComponent<CameraDepthToggle>();
        gameObject.AddComponent<UpdateFogValuesDV>();
        gameObject.AddComponent<CameraGraphicsUpdater>();
        gameObject.AddComponent<StreamingController>();
        // THIS ALWAYS HAS TO BE LAST FOR SOME REASON??????
    }
}
