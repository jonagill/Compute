using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ExtrusionVisibility : MonoBehaviour
{
    public Vector3 boundsSize = new Vector3(1, 1, 1);

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Mesh boundsMesh;

    public bool IsVisible { get; private set; }

    private SubExtrusion[] subExtrusions;
    public IEnumerable<SubExtrusion> SubExtrusions
    {
        get
        {
            return subExtrusions;
        }
    }

    private void Awake()
    {
        ExtrusionRenderer.AllExtrusions.Add(this);
        subExtrusions = GetComponentsInChildren<SubExtrusion>(true);
    }

    // Use this for initialization
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        boundsMesh = new Mesh();
        meshFilter.mesh = boundsMesh;
    }

    private void OnDestroy()
    {
        ExtrusionRenderer.AllExtrusions.Remove(this);

        if (boundsMesh != null)
        {
            Destroy(boundsMesh);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // TODO(jonagill): Actually calculate the size of the bounds
        boundsMesh.bounds = new Bounds(Vector3.zero, boundsSize);
    }

    private void OnDrawGizmos()
    {
        if (meshRenderer)
        {
            Gizmos.DrawWireCube(meshRenderer.bounds.center, meshRenderer.bounds.size);
        }
    }

    private void OnBecameVisible()
    {
        IsVisible = true;
    }

    private void OnBecameInvisible()
    {
        IsVisible = false;
    }
}
