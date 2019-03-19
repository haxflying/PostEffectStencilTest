using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostEffectTest : MonoBehaviour {

    public Shader postEffectShader;
    private Material postMat;


    void Start()
    {
        postMat = new Material(postEffectShader);
    }


    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        var buffer = RenderTexture.GetTemporary(Screen.width, Screen.height);
        Graphics.Blit(src, buffer);
        Graphics.SetRenderTarget(buffer.colorBuffer, src.depthBuffer);
        Graphics.Blit(src, postMat);
        Graphics.Blit(buffer, dst);
        RenderTexture.ReleaseTemporary(buffer);
    }
}
