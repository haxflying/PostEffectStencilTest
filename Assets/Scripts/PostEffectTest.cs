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
    private List<StencilMaskObject> maskList = new List<StencilMaskObject>();

    void Start () {

        postMat = new Material(postEffectShader);
        stencilCopy = new RenderTexture(Screen.width, Screen.height, 24);
        stencilCopy.name = "StencilCopy";

        maskList = new List<StencilMaskObject>(FindObjectsOfType<StencilMaskObject>());

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
        //cmd.Blit(BuiltinRenderTextureType.CurrentActive, stencilCopy);
        cmd.SetRenderTarget(stencilCopy);
        foreach (var obj in maskList)
        {
            cmd.DrawRenderer(obj.GetComponent<Renderer>(), obj.GetComponent<Renderer>().sharedMaterial, 0, 0);
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    { 
        if (postMat != null)
        {         
            var temp = RenderTexture.GetTemporary(Screen.width, Screen.height, 24);
            Graphics.Blit(src, temp);
            Graphics.SetRenderTarget(temp.colorBuffer, stencilCopy.depthBuffer);
            Graphics.Blit(src, postMat);
            Graphics.Blit(temp, dst);
            RenderTexture.ReleaseTemporary(temp);
        }
        else
        {
            Graphics.Blit(src, dst);
        }
    }
}
