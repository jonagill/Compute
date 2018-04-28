using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class ExtrusionRenderer : MonoBehaviour
{
    private readonly List<SubExtrusion> subExtrusionsToRender = new List<SubExtrusion>();

    private new Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void Update ()
    {
        RenderExtrusions();
	}

    private void RenderExtrusions()
    {
        var frustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
        CollectSubextrusionsToRender(frustumPlanes);
        RenderSubextrusions();
    }

    private void CollectSubextrusionsToRender(Plane[] frustumPlanes)
    {
        subExtrusionsToRender.Clear();
        foreach (var extrusion in ExtrusionManager.AllExtrusions)
        {
            // Only render any subextrusions if this extrusion is a) globally visible and b) visible to this particular camera
            if (extrusion.IsVisible && GeometryUtility.TestPlanesAABB(frustumPlanes, extrusion.WorldBounds))
            {
                subExtrusionsToRender.AddRange(extrusion.SubExtrusions);
            }
        }
    }

    private void RenderSubextrusions()
    {
        // TODO(jonagill): Special case ignored and highlighted extrusions somehow?
        var matrices = subExtrusionsToRender.Select(s => s.transform.localToWorldMatrix).ToArray();
        Graphics.DrawMeshInstanced(ExtrusionManager.Instance.cubeMesh, 0, ExtrusionManager.Instance.renderMaterial, matrices, matrices.Length, null, ShadowCastingMode.On, true, gameObject.layer, null);
    }

}
