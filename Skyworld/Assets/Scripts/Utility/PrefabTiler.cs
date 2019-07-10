using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTiler : MonoBehaviour {

    [SerializeField]
    GameObject prefab;

	// Use this for initialization
	void Start () {
        if (prefab == null && (prefab = (transform.childCount > 0 ? transform.GetChild(0).gameObject : null)) == null)
        {
            Debug.LogWarning("No GameObject prefab assigned to PrefabTiler.");
            return;
        }
        for (int i = 0; i < transform.localScale.x; i++)
        {
            for (int j = 0; j < transform.localScale.y; j++)
            {
                GameObject tileGo = Instantiate(prefab, transform);
                tileGo.transform.localPosition = new Vector2((i - (transform.localScale.x - 1) / 2f) / transform.localScale.x, (j - (transform.localScale.y - 1) / 2f) / transform.localScale.y);
            }
        }
	}
}
