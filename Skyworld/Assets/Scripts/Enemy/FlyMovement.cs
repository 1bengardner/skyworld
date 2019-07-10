using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class FlyMovement : EnemyMovement {

    public float speed;
    [SerializeField]
    WaypointProgressTracker progressTracker;

    protected override void Start()
    {
        base.Start();
        if (progressTracker == null)
        {
            Debug.LogError("Fly is missing a progress tracker!");
        }
    }
    
	protected override void Move () {
        rb.velocity = (progressTracker.target.position - transform.position).normalized * speed;
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));
        if (rb.velocity.x > 0 != facingRight)
        {
            Flip();
        }
    }
}
