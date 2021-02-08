using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererSetup : MonoBehaviour {

    public string layerName;
    public int order;
    
	void Start ()
    {
        TrailRenderer trail = GetComponent<TrailRenderer>();
        trail.sortingLayerName = layerName;
        trail.sortingOrder = order;
    }
}
