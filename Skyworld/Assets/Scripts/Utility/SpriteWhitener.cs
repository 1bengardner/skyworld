using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteWhitener : MonoBehaviour {
    
    [SerializeField]
    Material material;

    [Range(0, 1)]
    [SerializeField]
    float whiteValue;

    // Use this for initialization
    void Start () {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in srs)
        {
            SpriteRenderer newSr = Instantiate(sr, sr.transform.parent);
            Destroy(newSr.GetComponent<ColorIfKey>());
            newSr.material = material;
            newSr.color = new Color(1f, 1f, 1f, 0.25f);
            newSr.transform.SetAsFirstSibling();
        }
	}
}
