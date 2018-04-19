using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CubeRenderer : MonoBehaviour
{
    private ComputeShader compute;
    private Material material;

    private ComputeBuffer vertexBuffer;

    private int _generateKernelId;
    private int _vertexBufferId;

    private CommandBuffer commandBuffer;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void Initialize(ComputeShader compute, Material material)
    {
        if (this.compute == null)
        {
            this.compute = compute;
            this.material = material;

            _generateKernelId = compute.FindKernel("Generate");
            _vertexBufferId = Shader.PropertyToID("VertexBuffer");

            commandBuffer = new CommandBuffer();
            cam.AddCommandBuffer(CameraEvent.AfterForwardOpaque, commandBuffer);
        }
    }

    private void OnDestroy()
    {
        ReleaseBuffers();
    }

    void OnPreRender ()
    {
        if (commandBuffer != null)
        {
            EnsureBuffers();

            commandBuffer.Clear();
            commandBuffer.DispatchCompute(compute, _generateKernelId, 1, 1, 1);
            commandBuffer.DrawProcedural(Matrix4x4.identity, material, 0, MeshTopology.Triangles, 3);
        }
    }

    private void ReleaseBuffers()
    {
        if (vertexBuffer != null)
        {
            vertexBuffer.Release();
            vertexBuffer = null;
        }
    }

    private void EnsureBuffers()
    {
        if (vertexBuffer == null || vertexBuffer.count != 3)
        {
            ReleaseBuffers();

            vertexBuffer = new ComputeBuffer(3, sizeof(float) * 4);

            compute.SetBuffer(_generateKernelId, _vertexBufferId, vertexBuffer);
            material.SetBuffer(_vertexBufferId, vertexBuffer);
        }
    }

    private void DebugData()
    {
        Vector3[] pos = new Vector3[3];
        int[] indices = new int[3];

        vertexBuffer.GetData(pos, 0, 0, 3);
        Debug.LogWarning("Verts");
        for (int i = 0; i < pos.Length; i++)
        {
            Debug.Log(pos[i]);
        }
    }
}
