using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingSlimeMovement : SlimeMovement
{
    const float slideDelay = 0.1f;  // Time between player-proximity slides

    protected override void Start()
    {
        secondsBetweenSlides = 0f;
        base.Start();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && idleTime >= slideDelay)
            Slide();
    }
}
