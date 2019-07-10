using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMuteText : MonoBehaviour {

    public BackgroundMusic backgroundMusic;

    Text muteText;
    IEnumerator coroutine;
    
    void Awake ()
    {
        muteText = GetComponent<Text>();

        backgroundMusic.OnToggleMute += ShowMutedTextWrapper;
    }

    void ShowMutedTextWrapper()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = GetComponent<TextPeeker>().FlashText(muteText, backgroundMusic.muted ? "Music off" : "Music on");
        StartCoroutine(coroutine);
    }
}
