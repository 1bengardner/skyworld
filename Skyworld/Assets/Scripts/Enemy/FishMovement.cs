using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : JumpMovement
{
    [SerializeField]
    float secondsBetweenRandomFlips;
    float secondsSinceLastFlip = 0f;
    float flipVelocityThreshold = 1f;

    protected override void Move()
    {
        base.Move();
        if (rb.velocity.y > flipVelocityThreshold && secondsSinceLastFlip > secondsBetweenRandomFlips)
        {
            Flip();
            secondsSinceLastFlip = 0f;
        }
        else
        {
            secondsSinceLastFlip += Time.fixedDeltaTime;
        }
    }
}
