using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool Climbing
    {
        get { return m_Climb; }
    }
    public delegate void JumpAction();
    public event JumpAction OnJump;
    public Rigidbody2D mount;
    [SerializeField] private float m_MaxSpeed = 5f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float m_ClimbSpeed = 2.5f;                // The fastest the player can travel in the y axis.
    public float jumpForce = 400f;                                     // Amount of force added when the player jumps.
    [SerializeField] private float m_MaxHangTime = 0.5f;               // Most seconds the player can accelerate for after jumping.
    [SerializeField] private float m_PropulsionForce = 400f;           // Additional force added as the player remains in the air.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f; // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = true;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                 // A mask determining what is ground to the character
    [SerializeField] private LayerMask m_WhatIsLadder;                 // A mask determining what is a ladder
    [SerializeField] private AudioClip[] m_jumpClips;
    [SerializeField] private AudioClip[] m_footstepClips;

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedHeight = .1f; // Height of the overlap rect to determine if grounded
    const float k_GroundedWidth = .45f; // Width of the overlap rect to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .025f; // Radius of the overlap circle to determine if the player can stand up or climb
    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private AudioSource m_AudioSource;
    private bool m_FacingRight;  // For determining which way the player is currently facing.
    private float m_HangTime = 0f;      // For tracking the number of seconds the player has been in midair.
    private bool m_Ladder = false;      // Whether or not the player is near a ladder.
    private bool m_Climb = false;       // Whether or not the player is climbing.
    private bool m_CanPropel = false;
    private float m_TimeSinceLastFootstep = 0.0f;
    private float m_FallThroughVelocity = 10f;

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_AudioSource = GetComponent<AudioSource>();
        m_FacingRight = transform.localScale.x > 0 ? true : false;
    }
        
    // End propulsion if something is hit.
    private void OnCollisionEnter2D(Collision2D other)
    {
        m_CanPropel = other.collider.usedByEffector || other.collider.isTrigger ? m_CanPropel : false;
    }

    private void FixedUpdate()
    {
        // Set collision mode to avoid falling through platforms
        m_Rigidbody2D.collisionDetectionMode = Mathf.Abs(m_Rigidbody2D.velocity.y) > m_FallThroughVelocity ? CollisionDetectionMode2D.Continuous : CollisionDetectionMode2D.Discrete;

        m_Grounded = false;
        m_Ladder = false;

        // The player is grounded if an areacast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapAreaAll(
            m_GroundCheck.position + Vector3.left * (k_GroundedWidth / 2) + (Vector3.up * k_GroundedHeight / 2),
            m_GroundCheck.position + Vector3.right * (k_GroundedWidth / 2) + (Vector3.down * k_GroundedHeight / 2),
            m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject && !colliders[i].isTrigger)
                m_Grounded = true;
        }
        m_Anim.SetBool("Ground", m_Grounded);

        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", Mathf.Abs(m_Rigidbody2D.velocity.y));

        if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsLadder))
        {
            m_Ladder = true;
        }
        m_Anim.SetBool("Climb", m_Climb);

        m_Rigidbody2D.gravityScale = m_Climb ? 0 : 3;
    }


    public void Move(float hMove, float vMove, bool crouch, bool jump, bool propel)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch && m_Anim.GetBool("Crouch"))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (colliders[i] != null && !colliders[i].isTrigger)
                {
                    crouch = true;
                }
            }
        }

        // Set whether or not the character is crouching in the animator
        m_Anim.SetBool("Crouch", crouch);

        // make the player climb if they go up or down near a ladder, or left or right while on a ladder
        if ((m_Ladder && !crouch && vMove != 0) || (m_Ladder && m_Climb && !m_Grounded))
        {
            m_Climb = true;
            m_Anim.SetBool("Climb", m_Climb);
            m_Anim.SetFloat("Speed", Mathf.Abs(hMove));

            // Move the character
            m_Rigidbody2D.velocity = new Vector2(hMove * m_MaxSpeed / 2 + (mount ? mount.velocity.x : 0), vMove * m_ClimbSpeed);
        }
        //only control the player if grounded or airControl is turned on
        else if (m_Grounded || m_AirControl)
        {
            m_Climb = false;
            // Reduce the speed if crouching by the crouchSpeed multiplier
            hMove = (crouch ? hMove*m_CrouchSpeed : hMove);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(hMove));

            // Move the character
            m_Rigidbody2D.velocity = new Vector2(hMove * m_MaxSpeed + (mount ? mount.velocity.x : 0), m_Rigidbody2D.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (hMove > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
                // Otherwise if the input is moving the player left and the player is facing right...
            else if (hMove < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

        // If the player should jump...
        if (m_Grounded && jump && m_Anim.GetBool("Ground"))
        {
            m_HangTime = 0f;
            m_Grounded = false;
            m_Climb = false;
            m_CanPropel = true;
            m_Anim.SetBool("Ground", false);

            // Add a vertical force to the player.
            m_Rigidbody2D.AddForce(Vector2.up * jumpForce);

            // Play a random jump sound
            int i = UnityEngine.Random.Range(1, m_jumpClips.Length);
            m_AudioSource.PlayOneShot(m_jumpClips[i]);
            // Play a different sound next time
            AudioClip temp = m_jumpClips[i];
            m_jumpClips[i] = m_jumpClips[0];
            m_jumpClips[0] = temp;

            if (OnJump != null)
            {
                OnJump();
            }
        }
        // Start propelling the player in midair if jump is held.
        else if (!m_Climb && m_CanPropel && propel && m_HangTime < m_MaxHangTime)
        {
            m_HangTime += Time.deltaTime;
            // Add a vertical propulsion force to the player.
            m_Rigidbody2D.AddForce(Vector2.up * (jumpForce + m_PropulsionForce) * Time.fixedDeltaTime);
        }
        // End propulsion if the jump button is released.
        else
        {
            m_CanPropel = false;
        }

        float footStepDelay = 0.4166f / (m_MaxSpeed * Mathf.Abs(hMove));
        // Sound off footsteps if player is walking on ground
        if (m_Grounded && Mathf.Abs(m_Rigidbody2D.velocity.x - (mount ? mount.velocity.x : 0)) > 1.0f && m_TimeSinceLastFootstep > footStepDelay)
        {
            m_TimeSinceLastFootstep = 0.0f;
            // Play a random footstep sound
            int i = UnityEngine.Random.Range(1, m_footstepClips.Length);
            m_AudioSource.PlayOneShot(m_footstepClips[i], 0.5f);
            // Play a different sound next time
            AudioClip temp = m_footstepClips[i];
            m_footstepClips[i] = m_footstepClips[0];
            m_footstepClips[0] = temp;
        }
        else
        {
            m_TimeSinceLastFootstep += Time.fixedDeltaTime;
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}