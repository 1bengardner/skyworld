using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenOverlay : MonoBehaviour {

    Image fader;
    CanvasGroup cg;

    void Awake()
    {
        fader = GetComponent<Image>();
        cg = GetComponent<CanvasGroup>();
    }

    public void SetColor(Color color)
    {
        if (cg != null)
        {
            cg.alpha = color.a;
        }
        if (fader != null)
        {
            fader.color = color;
        }
    }

    // Fade the screen to a color over time.
    public IEnumerator Fade(Color color, float seconds = 1f)
    {
        if (cg != null)
        {
            yield return StartCoroutine(FadeCg(color.a, seconds));
        }
        if (fader != null)
        {
            yield return StartCoroutine(FadeImage(color, seconds));
        }
    }

    IEnumerator FadeImage(Color color, float seconds)
    {
        Color startColor = fader.color;
        float elapsed = 0f;
        while (elapsed <= seconds)
        {
            elapsed += Time.deltaTime;
            fader.color = Color.Lerp(startColor, color, elapsed / seconds);
            yield return null;
        }
        fader.color = color;
    }

    IEnumerator FadeCg(float alpha, float seconds)
    {
        float startAlpha = cg.alpha;
        float elapsed = 0f;
        while (elapsed <= seconds)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, alpha, elapsed / seconds);
            yield return null;
        }
        cg.alpha = alpha;
        cg.interactable = cg.alpha > 0f ? true : false;
    }
}
