using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostEffectTest : MonoBehaviour {

    public Shader postEffectShader;
    private Material postMat;
    private CommandBuffer cb_stencilCopy;
    private CommandBuffer cb_finalBlit;
    private Camera cam;
    private RenderTexture stencilCopy;

    void Start()
    {
        postMat = new Material(postEffectShader);
        stencilCopy = new RenderTexture(Screen.width, Screen.height, 24);
        stencilCopy.name = "stencilCopy";
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
