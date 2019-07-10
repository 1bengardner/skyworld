using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour {

    public PlayerHealth playerHealth;
    public GameObject heartPrefab;
    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty;

    RectTransform heartContainer;

    void Start()
    {
        heartContainer = GetComponent<RectTransform>();

        playerHealth.OnTakeDamage += UpdateHearts;
        playerHealth.OnHealDamage += UpdateHearts;
        playerHealth.OnGainHeart += AddHeart;

        for (int i = 0; i < playerHealth.maxHearts; i++)
        {
            GameObject heart = Instantiate(heartPrefab);
            heart.GetComponent<Image>().sprite = heartFull;
            heart.transform.SetParent(heartContainer, false);
            RectTransform rect = heart.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(rect.sizeDelta.x / 2 + i * 100f, -rect.sizeDelta.y / 2);
        }
    }

    IEnumerator AnimateHeart(Image heart)
    {
        float animationTime = 0.05f;
        float elapsedTime = 0f;
        float scale = 1.3f;
        while (elapsedTime < animationTime)
        {
            heart.rectTransform.localScale = Vector3.Lerp(Vector2.one, scale * Vector2.one, elapsedTime / animationTime);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        animationTime = 0.2f;
        elapsedTime = 0f;
        while (elapsedTime < animationTime)
        {
            heart.rectTransform.localScale = Vector3.Lerp(scale * Vector2.one, Vector2.one, elapsedTime / animationTime);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        heart.rectTransform.localScale = Vector2.one;
    }

    void UpdateHearts()
    {
        int fullHearts = playerHealth.health / 2;
        int halfHearts = playerHealth.health % 2;
        Image heart;
        for (int i = 0; i < fullHearts; i++)
        {
            heart = heartContainer.GetChild(i).GetComponent<Image>();
            heart.sprite = heartFull;
            StartCoroutine(AnimateHeart(heart));
        }
        for (int j = fullHearts; j < halfHearts + fullHearts; j++)
        {
            heart = heartContainer.GetChild(j).GetComponent<Image>();
            heart.sprite = heartHalf;
            StartCoroutine(AnimateHeart(heart));
        }
        for (int k = halfHearts + fullHearts; k < playerHealth.maxHearts; k++)
        {
            heartContainer.GetChild(k).GetComponent<Image>().sprite = heartEmpty;
        }
    }

    void AddHeart()
    {
        GameObject heart = Instantiate(heartPrefab);
        heart.GetComponent<Image>().sprite = heartEmpty;
        heart.transform.SetParent(heartContainer, false);
        RectTransform rect = heart.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(rect.sizeDelta.x / 2 + (playerHealth.maxHearts - 1) * 100f, -rect.sizeDelta.y / 2);
    }
}
