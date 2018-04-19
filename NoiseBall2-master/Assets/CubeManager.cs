using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour {
    public ComputeShader computeShader;
    public Material renderMaterial;

	void Start ()
    {
        AddRenderer(Camera.main);

#if UNITY_EDITOR
        // Would need to monitor if a new window is created
        foreach (var viewObj in UnityEditor.SceneView.sceneViews)
        {
            UnityEditor.SceneView sceneView = viewObj as UnityEditor.SceneView;
            AddRenderer(sceneView.camera);
        }
#endif
    }

    private void AddRenderer(Camera camera)
    {
        var renderer = camera.gameObject.AddComponent<CubeRenderer>();
        renderer.Initialize(computeShader, renderMaterial);
    }
}
