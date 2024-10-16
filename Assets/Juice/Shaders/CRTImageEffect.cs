using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CRTImageEffect : MonoBehaviour
{
    public Material crtMaterial;
    public LayerMask excludeLayer;
    private Camera mainCamera;
    private Camera uiCamera;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();

        // Create a separate camera for UI rendering to avoid conflicts with CRT effect
        GameObject uiCameraObject = new GameObject("UICamera");
        uiCamera = uiCameraObject.AddComponent<Camera>();
        uiCamera.CopyFrom(mainCamera);
        uiCamera.clearFlags = CameraClearFlags.Depth;
        uiCamera.cullingMask = excludeLayer;
        uiCamera.depth = mainCamera.depth + 1;
        uiCameraObject.transform.SetParent(mainCamera.transform);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (crtMaterial != null)
        {
            // Create a temporary render texture to avoid conflicts
            RenderTexture tempRT = RenderTexture.GetTemporary(
                source.width,
                source.height,
                0,
                source.format
            );
            Graphics.Blit(source, tempRT);
            Graphics.Blit(tempRT, destination, crtMaterial);
            RenderTexture.ReleaseTemporary(tempRT);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
