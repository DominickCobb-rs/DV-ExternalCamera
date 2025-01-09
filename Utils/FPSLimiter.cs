// Learned repeatedly enabling and disabling the camera creates an rendering problems for the main camera.
// running Camera.render() makes vegetationmanager not render vegetation
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class FPSLimiter : MonoBehaviour
{
    public float fps = 30; // Change this to add a settings option
    private Camera cam;
    private Coroutine renderCoroutine;
    private int rendered = 0;

    private void OnEnable()
    {
        cam = GetComponent<Camera>();
        cam.enabled = false;
        StartRenderCoroutine();
    }

    private void OnDisable()
    {
        cam.enabled = false;
        cam = null;
        StopRenderCoroutine();
    }

    public void StartRenderCoroutine()
    {
        if (renderCoroutine == null)
        {
            renderCoroutine = StartCoroutine(RenderAtFPS());
        }
    }

    void StopRenderCoroutine()
    {
        if (renderCoroutine != null)
        {
            StopCoroutine(renderCoroutine);
            renderCoroutine = null;
        }
    }


    IEnumerator RenderAtFPS()
    {
        if (cam == null)
        {
            Debug.Log("cam is null");
        }
        else if (transform.parent.gameObject == null)
        {
            Debug.Log("transform.parent.gameObject is null");
        }
        while (true)
        {
            if (transform.parent.gameObject.activeSelf)
                yield return null;
            if (rendered < 1)
            {
                rendered++;
                if (!cam.enabled)
                    cam.enabled = true;
                yield return null;
            }
            cam.enabled = false;
            rendered = 0;
            yield return new WaitForSeconds(1.0f / fps);
        }
    }
}
