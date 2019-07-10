using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTiler : MonoBehaviour {

    [SerializeField]
    SpriteRenderer templateTile;

	// Use this for initialization
	void Start () {
        if (templateTile == null && (templateTile = GetComponent<SpriteRenderer>()) == null)
        {
            Debug.LogWarning("No SpriteRenderer assigned to SpriteTiler.");
            return;
        }
        for (int i = 0; i < transform.localScale.x; i++)
        {
            for (int j = 0; j < transform.localScale.y; j++)
            {
                GameObject tileGo = new GameObject(templateTile.name + i.ToString() + j);
                tileGo.transform.parent = transform;
                tileGo.transform.localPosition = new Vector2((i - (transform.localScale.x - 1) / 2f) / transform.localScale.x, (j - (transform.localScale.y - 1) / 2f) / transform.localScale.y);
                SpriteRenderer sr = tileGo.AddComponent<SpriteRenderer>();
                // Set new tile attributes to that of template
                sr.sprite = templateTile.sprite;
                sr.color = templateTile.color;
                sr.flipX = templateTile.flipX;
                sr.flipY = templateTile.flipY;
                sr.sortingLayerID = templateTile.sortingLayerID;
                sr.sortingOrder = templateTile.sortingOrder;
            }
        }
	}
}
