using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour {

    [SerializeField]
    string colliderTag;
    [SerializeField] float force;
    [SerializeField]
    Vector2 forceDirection;
    [SerializeField]
    float delay = 0f;
    [SerializeField]
    string completionTrigger;
    [SerializeField]
    AudioClip completionSound;
    [SerializeField]
    GameObject completionPrefab;
    [SerializeField]
    bool destroyAfterUse = true;

    IEnumerator routine;
    Collider2D whoToBoost;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == colliderTag && !other.isTrigger)
        {
            whoToBoost = other;
            StartCoroutine("Boost");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == colliderTag)
        {
            whoToBoost = other;
            StopCoroutine("Boost");
        }
    }

    IEnumerator Boost()
    {
        yield return new WaitForSeconds(delay);
        whoToBoost.GetComponent<Rigidbody2D>().AddForce(forceDirection.normalized * force);
        PostBoost();
        if (destroyAfterUse)
        {
            Destroy(this);
        }
    }

    void PostBoost()
    {
        if (!string.IsNullOrEmpty(completionTrigger))
        {
            GetComponent<Animator>().SetTrigger(completionTrigger);
        }
        if (completionSound != null)
        {
            GetComponent<AudioSource>().PlayOneShot(completionSound);
        }
        if (completionPrefab != null)
        {
            Instantiate(completionPrefab, transform, false);
        }
    }
}
