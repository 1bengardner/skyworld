using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text), typeof(TextPeeker))]
public class UIInfo : MonoBehaviour {

    const float pulseDuration = 0.2f;
    const float pulseMagnitude = 4f;

    public string infoString
    {
        private get { return _infoString;  }
        set {
            _infoString = value;
            ShowText();
            GetComponent<AudioSource>().Play();
        }
    }
    string _infoString = "Command: key";
    Text t;
    TextPeeker tp;
    Outline o;
    Vector2 outlineDistance;

    IEnumerator pulseCoroutine;
    IEnumerator flashCoroutine;

    void Start () {
        t = GetComponent<Text>();
        tp = GetComponent<TextPeeker>();
        o = GetComponent<Outline>();
        outlineDistance = o.effectDistance;
    }
	
	void ShowText()
    {
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
        }
        pulseCoroutine = PulseText();
        StartCoroutine(pulseCoroutine);
    }

    IEnumerator PulseText()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = tp.FlashText(t, infoString);
        StartCoroutine(flashCoroutine);

        if (o != null)
        {
            float elapsed = 0f;
            Vector2 startDistance = outlineDistance * pulseMagnitude;
            while (elapsed <= pulseDuration)
            {
                o.effectDistance = Vector2.Lerp(startDistance, outlineDistance, elapsed / pulseDuration);
                yield return null;
                elapsed += Time.deltaTime;
            }
        }
    }
}
