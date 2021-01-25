using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIZoneName: MonoBehaviour
{
    public string infoString
    {
        private get { return _infoString; }
        set
        {
            _infoString = value;
            ShowText();
        }
    }
    string _infoString = "Zone Name";
    Text t;
    TextPeeker tp;
    IEnumerator flashCoroutine;

    void Start()
    {
        t = GetComponent<Text>();
        tp = GetComponent<TextPeeker>();
    }

    void ShowText()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = tp.FlashText(t, infoString);
        StartCoroutine(flashCoroutine);
    }
}
