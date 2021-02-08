using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererGate : MonoBehaviour {

    public Rigidbody2D trackedBody;
    // Must be above this velocity to show trail (reduces artifacts)
    public float minVelocity;
    TrailRenderer trail;
    float startingTime;

    // Use this for initialization
    void Start ()
    {
        trail = GetComponent<TrailRenderer>();
        startingTime = trail.time;
    }
	
	// Update is called once per frame
	void Update () {
        trail.time = trackedBody.velocity.magnitude > minVelocity ? startingTime : 0f;
    }
}
