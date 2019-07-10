using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMovement : EnemyMovement {

    [SerializeField] AudioClip jumpClip;
    [SerializeField] float jumpForce;
    [SerializeField] float groundDuration;
    [SerializeField] LayerMask whatIsGround;
    float groundTime = 0f;
    Transform groundCheck;
    Transform wallCheck;
    Transform reverseWallCheck;
    protected const float groundRadius = 1f;
    const float wallRadius = .05f;

    protected override void Start()
    {
        base.Start();
        wallCheck = transform.Find("WallCheck");
        reverseWallCheck = transform.Find("WallCheckR");
        groundCheck = transform.Find("GroundCheck");
    }

    protected override void Move()
    {
        bool facingWall = false;
        bool wallBehind = false;
        bool grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject && !colliders[i].isTrigger)
            {
                grounded = true;
                break;
            }
        }
        Collider2D[] wallColliders = Physics2D.OverlapCircleAll(wallCheck.position, wallRadius, whatIsGround);
        for (int i = 0; i < wallColliders.Length; i++)
        {
            if (wallColliders[i].gameObject != gameObject && !wallColliders[i].isTrigger && !wallColliders[i].usedByEffector)
            {
                facingWall = true;
                break;
            }
        }
        wallColliders = Physics2D.OverlapCircleAll(reverseWallCheck.position, wallRadius, whatIsGround);
        for (int i = 0; i < wallColliders.Length; i++)
        {
            if (wallColliders[i].gameObject != gameObject && !wallColliders[i].isTrigger && !wallColliders[i].usedByEffector)
            {
                wallBehind = true;
                break;
            }
        }

        // Jump
        if (groundTime >= groundDuration)
        {
            rb.AddForce(new Vector2(jumpForce * (facingRight ? 1 : -1), jumpForce));
            groundTime = 0f;
            audioSource.PlayOneShot(jumpClip);
        }
        // Don't jump
        else
        {
            // Flip if facing a wall and not upside down
            if (facingWall && !wallBehind)
            {
                Flip();
            }
            if (grounded)
            {
                groundTime += Time.fixedDeltaTime;
            }
        }
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }
}
