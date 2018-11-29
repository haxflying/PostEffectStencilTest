using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostEffectTest : MonoBehaviour {

    public Shader postEffectShader;
    private Material postMat;
    private CommandBuffer cmd;
    private Camera cam;
    private RenderTexture stencilCopy;

    void Start () {

        postMat = new Material(postEffectShader);
        //stencilCopy = Shader.PropertyToID("stencilCopy");
        stencilCopy = new RenderTexture(Screen.width, Screen.height, 24);
        stencilCopy.name = "StencilCopy";
        cam = GetComponent<Camera>();
        cmd = new CommandBuffer();
        cmd.name = "stencil copy";

        cam.AddCommandBuffer(CameraEvent.AfterForwardOpaque, cmd);
	}

    private void OnPreRender()
    {
        if (cmd == null)
            return;

        cmd.Clear();
        //cmd.SetRenderTarget(BuiltinRenderTextureType.CurrentActive, stencilCopy.depthBuffer);
        //cmd.SetRenderTarget(stencilCopy);
        cmd.Blit(BuiltinRenderTextureType.CurrentActive, stencilCopy);
        cmd.SetRenderTarget(stencilCopy);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    { 
        if (postMat != null)
        {         
            var temp = RenderTexture.GetTemporary(Screen.width, Screen.height, 24);
            Graphics.SetRenderTarget(temp.colorBuffer, src.depthBuffer);
            Graphics.Blit(temp, dst, postMat);
            RenderTexture.ReleaseTemporary(temp);
        }
        else
        {
            Graphics.Blit(src, dst);
        }
    }
}
