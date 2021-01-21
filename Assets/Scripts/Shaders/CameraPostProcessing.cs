using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraPostProcessing : MonoBehaviour
{
    public List<Material> shaders;
    // Start is called before the first frame update
    void Awake()
    {
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        foreach(Material m in shaders) {
            Graphics.Blit(src, dest, m);
        }
    }
}
