using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parasol : MonoBehaviour {

    public float propulsionForce;
    public Sprite openSprite;
    Sprite closedSprite;
    SpriteRenderer spriteRenderer;
    Rigidbody2D parentRb;
    Animator parentAnim;
    Animator anim;
    const float openVelocity = -1f;
    bool canOpenParasol;

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentRb = transform.parent.GetComponent<Rigidbody2D>();
        parentAnim = transform.parent.GetComponent<Animator>();
        anim = GetComponent<Animator>();
        closedSprite = spriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("Open");
        }

        spriteRenderer.enabled = Input.GetButton("Jump") && canOpenParasol ? true : false;
        
        if (parentRb.velocity.y < openVelocity)
        {
            spriteRenderer.sprite = openSprite;
        }
        else if (parentRb.velocity.y > 0f)
        {
            spriteRenderer.sprite = closedSprite;
        }
	}

    void FixedUpdate()
    {
        canOpenParasol = false;
        if (!parentAnim.GetBool("Ground") && !parentAnim.GetBool("Climb") && (GameManager.Instance == null || !GameManager.Instance.paused))
        {
            canOpenParasol = true;
        }

        if (parentRb.velocity.y < openVelocity && spriteRenderer.enabled)
        {
            parentRb.AddForce(Vector2.up * propulsionForce * Time.fixedDeltaTime);
        }
    }
}
