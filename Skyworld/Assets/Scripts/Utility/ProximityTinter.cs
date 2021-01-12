using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityTinter : MonoBehaviour {

    [SerializeField]
    Transform tintBasedOnDistanceFrom;
    [SerializeField]
    Color fromColor;
    [SerializeField]
    Color toColor;
    [SerializeField]
    [Range(0.0f, 100.0f)]
    float fromColorDistance;
    SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
	}   
	
	// Update is called once per frame
	void Update () {
        float realDistance = Vector3.Magnitude(transform.position - tintBasedOnDistanceFrom.position);
        float t = (fromColorDistance - realDistance) / fromColorDistance;
        sr.color = Color.Lerp(fromColor, toColor, t);
	}
}
