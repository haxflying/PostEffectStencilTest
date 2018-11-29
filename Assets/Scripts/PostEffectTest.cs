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
        //stencilCopy = Shader.PropertyToID("stencilCopy");
        stencilCopy = new RenderTexture(Screen.width, Screen.height, 24);
        postMat = new Material(postEffectShader);
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
        cmd.Blit(BuiltinRenderTextureType.CurrentActive, stencilCopy);
    }

    [ImageEffectOpaque]
    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if(postMat != null)
        {
            //Graphics.SetRenderTarget(stencilCopy);
            Graphics.Blit(src, dst, postMat);
        }
        else
        {
            Graphics.Blit(src, dst);
        }
    }
}
