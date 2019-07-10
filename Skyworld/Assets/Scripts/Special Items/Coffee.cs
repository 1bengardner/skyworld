using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Coffee : MonoBehaviour {

    [SerializeField]
    AudioClip soundClip;
    [SerializeField]
    float boostForce;
    [SerializeField]
    GameObject smokePuffPrefab;
    [SerializeField]
    LayerMask whatIsGround;
    
    public delegate void StateAction(CoffeeState state);
    public event StateAction OnChangeState;

    public enum CoffeeState { Grounded, Waiting, ReadyToBoost, Boosting, Boosted };
    CoffeeState _parentState;
    CoffeeState parentState
    {
        get
        {
            return _parentState;
        }
        set
        {
            _parentState = value;
            if (OnChangeState != null)
            {
                OnChangeState(parentState);
            }
        }
    }

    Rigidbody2D parentRb;
    AudioSource parentAudio;
    PlayerMovement parentMove;
    Transform groundCheck;
    bool jumpedOnce;
    float secondsSinceJump;
    const float k_GroundedHeight = .1f;
    const float k_GroundedWidth = .45f;
    const float k_BoostDelay = 0.1f;
    bool holdingJump;

    // Use this for initialization
    void Start ()
    {
        parentRb = GetComponentInParent<Rigidbody2D>();
        parentAudio = GetComponentInParent<AudioSource>();
        parentMove = GetComponentInParent<PlayerMovement>();
        groundCheck = transform.parent.Find("GroundCheck");
        parentMove.OnJump += OnJump;
    }

    private void OnJump()
    {
        parentState = CoffeeState.Waiting;
        secondsSinceJump = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (parentState == CoffeeState.Waiting)
        {
            secondsSinceJump += Time.deltaTime;
        }
        if (parentState == CoffeeState.ReadyToBoost && CrossPlatformInputManager.GetButtonDown("Jump") && !holdingJump && (GameManager.Instance == null || !GameManager.Instance.paused))
        {
            parentState = CoffeeState.Boosting;
        }
        holdingJump = false;
        if (CrossPlatformInputManager.GetButton("Jump"))
        {
            holdingJump = true;
        }
	}

    void FixedUpdate()
    {
        if (parentState == CoffeeState.Waiting && secondsSinceJump >= k_BoostDelay)
        {
            parentState = CoffeeState.ReadyToBoost;
        }

        else if (parentState == CoffeeState.Boosting)
        {
            parentRb.AddForce(Vector2.up * boostForce);
            parentRb.velocity = new Vector2(parentRb.velocity.x, Mathf.Clamp(parentRb.velocity.y, float.NegativeInfinity, 1f));
            Instantiate(smokePuffPrefab, transform.parent.position + 0.5f * Vector3.down, Quaternion.identity);
            parentAudio.PlayOneShot(soundClip);
            parentState = CoffeeState.Boosted;
        }

        Collider2D[] colliders = Physics2D.OverlapAreaAll(
                groundCheck.position + Vector3.left * (k_GroundedWidth / 2) + (Vector3.up * k_GroundedHeight / 2),
                groundCheck.position + Vector3.right * (k_GroundedWidth / 2) + (Vector3.down * k_GroundedHeight / 2),
                whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != parentRb.gameObject && !colliders[i].isTrigger && parentState != CoffeeState.Waiting)
            {
                parentState = CoffeeState.Grounded;
                break;
            }
        }
    }
}
