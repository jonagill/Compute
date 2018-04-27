using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class ExtrusionRenderer : MonoBehaviour {

    public static readonly List<ExtrusionVisibility> AllExtrusions = new List<ExtrusionVisibility>();

    private readonly List<SubExtrusion> subExtrusionsToRender = new List<SubExtrusion>();

    public Mesh cubeMesh;
    public Material material;

	void Update ()
    {
        RenderExtrusions();
	}

    private void RenderExtrusions()
    {
        CollectSubextrusionsToRender();
        RenderSubextrusions();
    }

    private void CollectSubextrusionsToRender()
    {
        subExtrusionsToRender.Clear();
        foreach (var extrusion in AllExtrusions)
        {
            if (extrusion.IsVisible)
            {
                subExtrusionsToRender.AddRange(extrusion.SubExtrusions);
            }
        }
    }

    private void RenderSubextrusions()
    {
        // TODO(jonagill): Special case ignored extrusions somehow?
        var matrices = subExtrusionsToRender.Select(s => s.transform.localToWorldMatrix).ToArray();
        Graphics.DrawMeshInstanced(cubeMesh, 0, material, matrices, matrices.Length, null, ShadowCastingMode.On, true, gameObject.layer, null);
    }

}
