using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrusionManager : MonoBehaviour {
    public static readonly List<ExtrusionVisibility> AllExtrusions = new List<ExtrusionVisibility>();

    public static ExtrusionManager Instance { get; private set; }

    public Mesh cubeMesh;
    public ComputeShader computeShader;
    public Material renderMaterial;

    private void Awake()
    {
        Instance = this;
    }

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
        camera.gameObject.AddComponent<ExtrusionRenderer>();
    }
}
