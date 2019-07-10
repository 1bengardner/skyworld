using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationMark : MonoBehaviour
{
    [SerializeField]
    GameObject exclamationPrefab;
    [SerializeField]
    Vector2 offset;
    bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !triggered)
        {
            triggered = true;
            Vector2 extents = GetComponent<SpriteRenderer>().sprite.bounds.extents;
            Instantiate(exclamationPrefab
                , (Vector2)transform.position + Vector2.Scale(Vector2.up * (extents.y + 0.25f) + offset, transform.localScale)
                , Quaternion.identity
                , transform);
        }
    }
}
