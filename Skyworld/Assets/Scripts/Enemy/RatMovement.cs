using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatMovement : PlatformMovement
{
    [SerializeField] float minSecondsBeforeFlip;
    float secondsSinceLastFlip = 0f;
    float secondsSinceLastFlipAttempt = 0f;

    const float chanceToFlip = 0.1f;
    const float secondsBetweenFlipAttempts = 0.25f;

    protected override void Move()
    {
        base.Move();
        if (secondsSinceLastFlip > minSecondsBeforeFlip && secondsSinceLastFlipAttempt > secondsBetweenFlipAttempts)
        {
            secondsSinceLastFlipAttempt = 0f;
            if (Random.value < chanceToFlip)
            {
                secondsSinceLastFlip = 0f;
                Flip();
            }
        }
        else
        {
            secondsSinceLastFlipAttempt += Time.deltaTime;
            secondsSinceLastFlip += Time.deltaTime;
        }
    }
}
