using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokePuff : MonoBehaviour {

    [SerializeField]
    bool rotate;

    void Start () {
        if (rotate)
            transform.Rotate(Vector3.forward * Random.value * 360f);
    }
}
