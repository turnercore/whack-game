using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CRTImageEffect : MonoBehaviour
{
    public Material crtMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (crtMaterial != null)
        {
            Graphics.Blit(source, destination, crtMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
