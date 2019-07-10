using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : EnemyMovement {

    public float maxSpeed;
    [SerializeField] protected LayerMask whatIsGround;
    Transform platformCheck;
    Transform wallCheck;
    Transform reverseWallCheck;
    const float platformRadius = .1f;
    const float wallRadius = .05f;
    protected float speed;

    protected override void Start()
    {
        base.Start();
        speed = maxSpeed;
        platformCheck = transform.Find("PlatformCheck");
        wallCheck = transform.Find("WallCheck");
        reverseWallCheck = transform.Find("WallCheckR");
    }

    protected override void Move()
    {
        bool steady = false;
        bool wallBehind = false;
        float angle = transform.rotation.z;
        // Check for ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(platformCheck.position, platformRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject && !colliders[i].isTrigger)
            {
                // There is a platform below
                steady = true;
                break;
            }
        }
        // Check for walls
        colliders = Physics2D.OverlapCircleAll(wallCheck.position, wallRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject && !colliders[i].isTrigger)
            {
                // There is a wall ahead
                steady = false;
                break;
            }
        }
        colliders = Physics2D.OverlapCircleAll(reverseWallCheck.position, wallRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject && !colliders[i].isTrigger)
            {
                wallBehind = true;
                break;
            }
        }
        // Flip if about to fall off platform or about to hit wall and not upside down
        if (!steady
            && !wallBehind
            && ((angle >= 0f && angle < 0.5f)
                || (angle < 0f && angle > -0.5f))
           )
        {
            Flip();
        }
        if ((angle >= 0f && angle < 0.5f)
                || (angle < 0f && angle > -0.5f))
        {
            rb.velocity = new Vector2(speed * (facingRight ? 1 : -1), rb.velocity.y);
            anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        }
    }
}
