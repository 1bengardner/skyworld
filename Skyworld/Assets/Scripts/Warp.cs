using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Warp : MonoBehaviour {

    public Transform destination;
    Collider2D objectToTransport;
    bool isOperational;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isOperational = true;
            objectToTransport = other;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isOperational = false;
        }
    }

    void Update()
    {
        if (isOperational && CrossPlatformInputManager.GetAxisRaw("Vertical") == 1f)
        {
            Transport();
        }
    }

    void Transport()
    {
        objectToTransport.transform.position = destination.position + 0.5f * Vector3.up;
    }
}
