using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPeeker : MonoBehaviour
{
    public enum Animation { Fade, Drop };
    [SerializeField] Animation anim;
    [SerializeField] float displayDuration = 1f;
    [SerializeField] float outDuration = 0.25f;
    [SerializeField] float inDuration = 0f;

    public IEnumerator FlashText(Text t, string s)
    {
        t.enabled = true;
        t.text = s;
        float elapsed = 0f;

        float offset = 50;
        float originalY = t.rectTransform.anchoredPosition.y;

        if (GetComponent<Outline>() != null)
        {
            Color color = GetComponent<Outline>().effectColor;
            GetComponent<Outline>().effectColor = new Color(color.r, color.g, color.b, 1f);
        }

        while (elapsed < inDuration)
        {
            switch (anim)
            {
                case Animation.Drop:
                    t.rectTransform.anchoredPosition = new Vector2(t.rectTransform.anchoredPosition.x
                        , Mathf.Lerp(originalY + offset, originalY, Mathf.Sqrt(elapsed / inDuration))
                    );
                    break;
            }
            t.color = new Color(t.color.r
                , t.color.g
                , t.color.b
                , Mathf.Lerp(0f, 1f, elapsed / inDuration)
            );
            yield return null;
            elapsed += Time.deltaTime;
        }

        t.rectTransform.anchoredPosition = new Vector2(t.rectTransform.anchoredPosition.x, originalY);
        t.color = new Color(t.color.r
            , t.color.g
            , t.color.b
            , 1f
        );
        yield return new WaitForSeconds(displayDuration);

        if (GetComponent<Outline>() != null)
        {
            Color color = GetComponent<Outline>().effectColor;
            GetComponent<Outline>().effectColor = new Color(color.r, color.g, color.b, 0f);
        }
        elapsed = 0f;
        while (elapsed < outDuration)
        {
            t.color = new Color(t.color.r
                , t.color.g
                , t.color.b
                , Mathf.Lerp(1f, 0f, elapsed / outDuration)
            );
            yield return null;
            elapsed += Time.deltaTime;
        }

        t.color = new Color(t.color.r
            , t.color.g
            , t.color.b
            , 0f
        );
        t.enabled = false;
    }
}
