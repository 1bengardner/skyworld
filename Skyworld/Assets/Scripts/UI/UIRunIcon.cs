using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRunIcon : MonoBehaviour
{
    public PlayerUserControl playerUserControl;
    public Sprite walkSprite;
    public Sprite runSprite;

    Image runIcon;
    IEnumerator coroutine;

    void Awake ()
    {
        runIcon = GetComponent<Image>();

        playerUserControl.OnRunLock += ShowRunIconWrapper;

        if (walkSprite == null)
        {
            walkSprite = GetComponent<Image>().sprite;
        }
    }

    void ShowRunIconWrapper()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = ShowRunIcon();
        StartCoroutine(coroutine);
    }

    IEnumerator ShowRunIcon()
    {
        float displayDuration = 1f;
        float fadeDuration = 0.25f;

        runIcon.color = new Color(runIcon.color.r
                , runIcon.color.g
                , runIcon.color.b
                , 1f);
        runIcon.enabled = true;
        runIcon.sprite = runIcon.sprite == walkSprite ? runSprite : walkSprite;
        yield return new WaitForSeconds(displayDuration);

        float elapsed = 0f;
        while (elapsed <= fadeDuration)
        {
            elapsed += Time.deltaTime;
            runIcon.color = new Color(runIcon.color.r
                , runIcon.color.g
                , runIcon.color.b
                , Mathf.Lerp(1f, 0f, elapsed / fadeDuration));
            yield return null;
        }
        runIcon.enabled = false;
    }
}
