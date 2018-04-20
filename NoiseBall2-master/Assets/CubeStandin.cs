using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeStandin : MonoBehaviour {

    public static readonly List<CubeStandin> AllCubes = new List<CubeStandin>();

	void Start ()
    {
        AllCubes.Add(this);        
	}

    private void OnDestroy()
    {
        AllCubes.Remove(this);
    }
}
