using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPeeker : MonoBehaviour
{
    [SerializeField] float displayDuration = 1f;
    [SerializeField] float fadeOutDuration = 0.25f;
    [SerializeField] float fadeInDuration = 0f;

    public IEnumerator FlashText(Text t, string s)
    {
        t.enabled = true;
        t.text = s;
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            t.color = new Color(t.color.r
                , t.color.g
                , t.color.b
                , Mathf.Lerp(0f, 1f, elapsed / fadeInDuration));
            yield return null;
            elapsed += Time.deltaTime;
        }

        t.color = new Color(t.color.r
                , t.color.g
                , t.color.b
                , 1f);
        yield return new WaitForSeconds(displayDuration);
        
        elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            t.color = new Color(t.color.r
                , t.color.g
                , t.color.b
                , Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration));
            yield return null;
            elapsed += Time.deltaTime;
        }

        t.color = new Color(t.color.r
                , t.color.g
                , t.color.b
                , 0f);
        t.enabled = false;
    }
}
