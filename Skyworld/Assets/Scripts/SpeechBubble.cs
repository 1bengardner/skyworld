using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour {

    [SerializeField]
    GameObject bubblePrefab;
    [SerializeField]
    Vector2 offset;
    Animator speechBubble;
    bool inRange = false;
    string boolName = "Visible";
    Collider2D coll;

    void Start () {
        coll = GetComponent<Collider2D>();
        Vector2 extents = GetComponent<SpriteRenderer>().sprite.bounds.extents;
		speechBubble = Instantiate(bubblePrefab
			, (Vector2)transform.position + Vector2.Scale(new Vector2(-extents.x, extents.y) + offset, transform.localScale)
			, Quaternion.identity
			, transform).GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
        }
    }

    private void Update()
    {
        if (inRange && !DialogueManager.Instance.isDisplaying && coll.enabled)
        {
            speechBubble.SetBool(boolName, true);
        }
        else
        {
            speechBubble.SetBool(boolName, false);
        }
    }
}
