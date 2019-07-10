using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgePlatform : MonoBehaviour {

    [SerializeField]
    float secondsUntilBroken;
    [SerializeField]
    AudioClip breakClip;
    AudioSource audioSource;
    float secondsElapsed;
    bool timerOn;
    Vector3 initialPosition;
    Quaternion initialRotation;
    Rigidbody2D rb;
    Animator anim;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        if (timerOn)
        {
            if (secondsElapsed >= secondsUntilBroken)
            {
                Break();
                timerOn = false;
            }
            secondsElapsed += Time.fixedDeltaTime;
        }
        else
        {
            secondsElapsed = 0f;
        }
    }
	
	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<Rigidbody2D>().velocity.y < -5f)
            {
                Break();
            }
            else
            {
                timerOn = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            timerOn = false;
        }
    }

    void Break()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        anim.SetTrigger("Break");
        audioSource.PlayOneShot(breakClip);
    }

    void Respawn()
    {
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<BoxCollider2D>().enabled = true;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        anim.SetTrigger("Respawn");
    }

    void OnDisable()
    {
        Respawn();
    }
}
