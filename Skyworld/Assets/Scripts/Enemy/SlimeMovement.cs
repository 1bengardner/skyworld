using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : PlatformMovement
{
    [SerializeField] AudioClip slideClip;
    [SerializeField] float slideForce;
    [SerializeField] protected float secondsBetweenSlides;
    [SerializeField] float secondsBetweenBursts; // Simulate slimelike behaviour
    [SerializeField] float minSpeed;
    protected float idleTime = 0f;
    float slowTime = 0f;
    bool sliding = false;
    float originalTimeBetweenClips;

    protected override void Start()
    {
        base.Start();
        originalTimeBetweenClips = timeBetweenClips;
    }

    protected override void Move()
    {
        // Slide
        if (secondsBetweenSlides > 0 && idleTime >= secondsBetweenSlides)
        {
            Slide();
        }
        else if (!(sliding && Mathf.Abs(rb.velocity.x) > 0.05f))
        {
            timeBetweenClips = originalTimeBetweenClips;
            anim.SetBool("Sliding", false);
            sliding = false;
            // Move fast every framesBetweenMovement frames
            if (slowTime >= secondsBetweenBursts)
            {
                slowTime = 0f;
                speed = maxSpeed;
            }
            else
            {
                speed = minSpeed;
            }
            base.Move();
            slowTime += Time.fixedDeltaTime;
            idleTime += Time.fixedDeltaTime;
        }
    }

    protected void Slide()
    {
        anim.SetBool("Sliding", true);
        sliding = true;
        rb.AddForce(Vector2.right * slideForce * (facingRight ? 1 : -1));
        idleTime = 0f;
        audioSource.PlayOneShot(slideClip);
        timeBetweenClips = 0f;
    }
}
