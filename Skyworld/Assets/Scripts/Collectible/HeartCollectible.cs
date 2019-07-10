using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCollectible : MonoBehaviour
{
    [SerializeField]
    int heartsHealed;
    [SerializeField]
    AudioClip collectClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.GetComponent<PlayerHealth>().health < other.GetComponent<PlayerHealth>().maxHearts * 2)
        {
            StartCoroutine(Collect(other));
        }
    }

    IEnumerator Collect(Collider2D collector)
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Animator>().SetTrigger("Collect");
        collector.GetComponent<AudioSource>().PlayOneShot(collectClip);
        yield return StartCoroutine(collector.GetComponent<PlayerHealth>().Heal(heartsHealed * 2));
        yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false);
    }
}
