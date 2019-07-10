using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour {

    [SerializeField]
    protected bool facingRight = false;
    [SerializeField]
    protected AudioClip moveClip;
    [SerializeField]
    protected float timeBetweenClips;
    float clipTimer = 0f;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected AudioSource audioSource;
    const float timeBetweenFlips = 0.33f;
    float flipTimer = 0f;

	// Use this for initialization
	protected virtual void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = moveClip;
    }
	
	// Update is called once per frame
	protected virtual void FixedUpdate () {
        if (true)//GameManager.Instance == null || !GameManager.Instance.paused)
        {
            flipTimer += Time.fixedDeltaTime;
            Move();
            clipTimer += Time.fixedDeltaTime;
            if (clipTimer > timeBetweenClips && timeBetweenClips != 0f && !audioSource.isPlaying)
            {
                if (audioSource.clip != null)
                {
                    audioSource.Play();
                }
                clipTimer = 0f;
            }
        }
        else
        {
            Stop();
        }
	}

    protected abstract void Move();

    protected void Flip()
    {
        if (flipTimer > timeBetweenFlips)
        {
            flipTimer = 0f;
            facingRight = !facingRight;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }

    protected void Stop()
    {
        rb.velocity = Vector2.zero;
    }
}
