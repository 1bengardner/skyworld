using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

[RequireComponent(typeof(ObjectResetter))]
public class ResetOnEnable : MonoBehaviour {

	void OnEnable()
    {
        GetComponent<ObjectResetter>().DelayedReset(0);
    }

    void Reset()
    {
        // Empty
    }
}
