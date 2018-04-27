using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillRenderTest : MonoBehaviour {

    public Vector3 boundsSize = new Vector3(1, 1, 1);

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Mesh mesh;

	// Use this for initialization
	void Start ()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        mesh = new Mesh();
        meshFilter.mesh = mesh;
	}
	
	// Update is called once per frame
	void Update ()
    {
        mesh.bounds = new Bounds(Vector3.zero, boundsSize);
	}

    private void OnDrawGizmos()
    {
        if (meshRenderer)
        {
            Gizmos.DrawWireCube(meshRenderer.bounds.center, meshRenderer.bounds.size);
        }
    }

    private void OnWillRenderObject()
    {
        Debug.Log("WillRender: " + gameObject.name + " " + Time.time);
    }
}
