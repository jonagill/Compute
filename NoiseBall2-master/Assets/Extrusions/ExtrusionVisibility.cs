using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class ExtrusionVisibility : MonoBehaviour
{
    public Vector3 boundsSize = new Vector3(1, 1, 1);

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Mesh boundsMesh;

    public bool IsVisible
    {
        get
        {
            return meshRenderer.isVisible;
        }
    }

    public Bounds WorldBounds
    {
        get
        {
            return meshRenderer.bounds;
        }
    }

    public IEnumerable<SubExtrusion> SubExtrusions
    {
        get
        {
            IEnumerable<SubExtrusion> allSubExtrusions = new SubExtrusion[0];
            for (var i = 0; i < typedSubExtrusions.Length; i++)
            {
                if (typedSubExtrusions[i] != null)
                {
                    allSubExtrusions = allSubExtrusions.Concat(typedSubExtrusions[i]);
                }
            }

            return allSubExtrusions;
        }
    }

    private List<SubExtrusion>[] typedSubExtrusions = new List<SubExtrusion>[(int) SubExtrusion.Type.Count];

    private void Awake()
    {
        ExtrusionManager.AllExtrusions.Add(this);

        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        boundsMesh = new Mesh();
        meshFilter.mesh = boundsMesh;

        CacheSubextrusions();
    }

    private void OnDestroy()
    {
        ExtrusionManager.AllExtrusions.Remove(this);

        if (boundsMesh != null)
        {
            Destroy(boundsMesh);
        }
    }

    private void OnDrawGizmos()
    {
        if (meshRenderer)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(meshRenderer.bounds.center, meshRenderer.bounds.size);
        }

        if (Application.isPlaying)
        {
            // TODO(jonagill): Remove me when not debugging bounds
            UpdateBounds();
        }
    }

    private void CacheSubextrusions()
    {
        var subExtrusions = GetComponentsInChildren<SubExtrusion>(true);
        foreach (var subExtrusion in subExtrusions)
        {
            AddSubExtrusion(subExtrusion, false);
        }

        UpdateBounds();
    }

    private void AddSubExtrusion(SubExtrusion subExtrusion, bool updateBounds = true)
    {
        var typeIndex = (int) subExtrusion.type;
        if (typedSubExtrusions[typeIndex] == null)
        {
            typedSubExtrusions[typeIndex] = new List<SubExtrusion>();
        }

        typedSubExtrusions[typeIndex].Add(subExtrusion);

        if (updateBounds)
        {
            UpdateBounds();
        }
    }

    private void RemoveSubExtrusion(SubExtrusion subExtrusion, bool updateBounds = true)
    {
        var typeIndex = (int)subExtrusion.type;
        if (typedSubExtrusions[typeIndex] != null)
        {
            typedSubExtrusions[typeIndex].Remove(subExtrusion);
        }

        if (updateBounds)
        {
            UpdateBounds();
        }
    }

    private void UpdateBounds()
    {
        var bounds = new Bounds(Vector3.zero, Vector3.zero);

        // Track the min and max values found across all subextrusions and calculate the bounds ourselves.
        // This is about 20% faster than calling Bounds.Encapsulate() for each subextrusion
        var localMin = Vector3.zero;
        var localMax = Vector3.zero;

        foreach (var subExtrusion in SubExtrusions)
        {
            var extrusionToLocal = transform.worldToLocalMatrix * subExtrusion.transform.localToWorldMatrix;
            var localBounds = subExtrusion.bounds.Transform(extrusionToLocal);

            var boundsMin = localBounds.min;
            localMin.x = Mathf.Min(localMin.x, boundsMin.x);
            localMin.y = Mathf.Min(localMin.y, boundsMin.y);
            localMin.z = Mathf.Min(localMin.z, boundsMin.z);

            var boundsMax = localBounds.max;
            localMax.x = Mathf.Max(localMax.x, boundsMax.x);
            localMax.y = Mathf.Max(localMax.y, boundsMax.y);
            localMax.z = Mathf.Max(localMax.z, boundsMax.z);
        }

        bounds.center = (localMin + localMax) * .5f;
        bounds.size = (localMax - localMin);

        boundsMesh.bounds = bounds;
    }
}
