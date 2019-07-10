using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionParticles : MonoBehaviour {

    [SerializeField]
    AudioClip breakClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GetComponent<SpriteRenderer>().enabled = false;
            BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
            
            foreach (BoxCollider2D collider in colliders)
            {
                collider.enabled = false;
            }

            ParticleSystem ps = GetComponent<ParticleSystem>();
            ps.Play();
            other.GetComponent<AudioSource>().PlayOneShot(breakClip);
            //Destroy(gameObject, ps.main.duration);
        }
    }

    // Reset when map is re-entered
    void OnEnable()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();

        foreach (BoxCollider2D collider in colliders)
        {
            collider.enabled = true;
        }
    }
}
