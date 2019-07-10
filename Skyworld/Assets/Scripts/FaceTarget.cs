using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTarget : MonoBehaviour {

    public Transform target;
    public bool facingRight;
    float proximity = 0.2f;
	
	// Update is called once per frame
	void Update () {
		if ((target.position.x - transform.position.x > proximity && !facingRight) ||
            (transform.position.x - target.position.x > proximity && facingRight))
        {
            Reverse();
        }
	}

    void Reverse()
    {
        facingRight = !facingRight;
        Vector2 scale = transform.localScale;
        scale.x = -scale.x;
        transform.localScale = scale;
    }
}
