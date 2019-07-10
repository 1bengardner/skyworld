using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinecartDestroyer : MonoBehaviour {
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Vehicle")
        {
            StartCoroutine(other.GetComponent<UnityStandardAssets.Utility.ObjectResetter>().ResetCoroutine(2f));
        }
    }

}
