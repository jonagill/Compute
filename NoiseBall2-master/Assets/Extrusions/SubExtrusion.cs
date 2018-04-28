using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubExtrusion : MonoBehaviour
{
    public enum Type
    {
        Box = 0,
        Sphere,
        Cylinder,
        Pyramid,
        Wedge,
        Count
    }

    public Type type = Type.Box;
    public Color color;
    public Bounds bounds = new Bounds(Vector3.one * .5f, Vector3.one * .5f);

    private void OnDrawGizmosSelected()
    {
        // Size calculation is probably wrong but good enough for now
        var worldBounds = transform.TransformBounds(bounds);

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(worldBounds.center, worldBounds.size);
    }
}
