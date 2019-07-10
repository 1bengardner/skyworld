using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorIfKey : MonoBehaviour {

    public Zone prefKeyZone;
    float missingKeyAlpha = 0.1f;

    void Start()
    {
        if (!PlayerPrefs.HasKey(prefKeyZone.id))
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, missingKeyAlpha);
        }
    }
}
