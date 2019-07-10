using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadybugMovement : PlatformMovement {

    [SerializeField] AudioClip flyClip;
    [SerializeField] float maxFlightSpeed;
    [SerializeField] float groundDuration;
    [SerializeField] float flightDuration;
    float groundTime = 0f;
    float flightTime = 0f;
    Transform flyCheck;
    const float flyRadius = 0.4f;
    Transform groundCheck;
    float groundRadius = .1f;
    delegate void MovementAction();
    MovementAction movementHandler;

    protected override void Start()
    {
        base.Start();
        flyCheck = transform.Find("FlyCheck");
        groundCheck = transform.Find("GroundCheck");
    }

    protected override void Move()
    {
        bool hasRoomToFly = true;
        Collider2D[] flyColliders = Physics2D.OverlapCircleAll(flyCheck.position, flyRadius, whatIsGround);
        for (int i = 0; i < flyColliders.Length; i++)
        {
            if (flyColliders[i].gameObject != gameObject && !flyColliders[i].isTrigger)
            {
                // There is a wall above
                hasRoomToFly = false;
                break;
            }
        }
        bool grounded = false;
        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, whatIsGround);
        for (int i = 0; i < groundColliders.Length; i++)
        {
            if (groundColliders[i].gameObject != gameObject && !groundColliders[i].isTrigger)
            {
                grounded = true;
                break;
            }
        }

        // Movement state selection

        // Fly up
        if (hasRoomToFly && ((movementHandler == base.Move && groundTime > groundDuration) ||
                             (movementHandler == FlyUp && flightTime < flightDuration)))
        {
            movementHandler = FlyUp;
            groundTime = 0f;
            flightTime += Time.fixedDeltaTime;
        }
        // Fly down
        else if ((movementHandler == FlyUp && (!hasRoomToFly || flightTime > flightDuration)) ||
                 (movementHandler == FlyDown && !grounded))
        {
            movementHandler = FlyDown;
        }
        // Grounded
        else
        {
            movementHandler = base.Move;
            flightTime = 0f;
            groundTime += Time.fixedDeltaTime;
            audioSource.clip = moveClip;
            audioSource.loop = false;
        }

        movementHandler();
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("vSpeed", Mathf.Abs(rb.velocity.y));
    }

    void FlyUp()
    {
        rb.velocity = new Vector2(0, maxFlightSpeed);
        audioSource.clip = flyClip;
        audioSource.loop = true;
    }

    void FlyDown()
    {
        rb.velocity = new Vector2(0, -maxFlightSpeed);
        audioSource.clip = flyClip;
        audioSource.loop = true;
    }
}
