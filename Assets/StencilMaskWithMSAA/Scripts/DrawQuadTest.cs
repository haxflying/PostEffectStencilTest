using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DrawQuadTest : MonoBehaviour {

    public Material quadMat;
    static Mesh s_Quad;
    public static Mesh quad
    {
        get
        {
            if (s_Quad != null)
                return s_Quad;

            var vertices = new[]
            {
                new Vector3(-0.5f, -0.5f, 0f),
                new Vector3(0.5f,  0.5f, 0f),
                new Vector3(0.5f, -0.5f, 0f),
                new Vector3(-0.5f,  0.5f, 0f)
            };

            var uvs = new[]
            {
                new Vector2(0f, 0f),
                new Vector2(1f, 1f),
                new Vector2(1f, 0f),
                new Vector2(0f, 1f)
            };

            var indices = new[] { 0, 1, 2, 1, 0, 3 };

            s_Quad = new Mesh
            {
                vertices = vertices,
                uv = uvs,
                triangles = indices
            };
            s_Quad.RecalculateNormals();
            s_Quad.RecalculateBounds();

            return s_Quad;
        }
    }

    private CommandBuffer cb;

    private Camera cam;

    private void OnEnable()
    {
        cam = GetComponent<Camera>();

        cb = new CommandBuffer();
        cb.name = "Draw Quad!!";

        cam.AddCommandBuffer(CameraEvent.AfterForwardOpaque, cb);     
        cb.DrawMesh(quad, Matrix4x4.identity, quadMat);
    }  
}
