using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundsExtensions
{
    public static Bounds Transform(this Bounds bounds, Matrix4x4 transform)
    {
        var center = transform.MultiplyPoint3x4(bounds.center);

        // transform the local extents' axes
        var extents = bounds.extents;
        
        var axisX = transform.MultiplyVector(new Vector3(extents.x, 0, 0));
        var axisY = transform.MultiplyVector(new Vector3(0, extents.y, 0));
        var axisZ = transform.MultiplyVector(new Vector3(0, 0, extents.z));

        // sum their absolute value to get the world extents
        extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
        extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
        extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

        return new Bounds { center = center, extents = extents };
    }
}
