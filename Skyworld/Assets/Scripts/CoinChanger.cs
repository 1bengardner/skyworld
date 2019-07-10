using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinChanger : MonoBehaviour {

    [SerializeField]
    int numberOfCoinsToChange;
    [SerializeField]
    int newWorth;
    [SerializeField]
    Color newColor;
    [SerializeField]
    Sprite newSprite;
    
	void Start () {
        if (newSprite == null)
        {
            newSprite = transform.GetChild(1).GetComponent<SpriteRenderer>().sprite;
        }

        int[] childIndices = RandomSample(numberOfCoinsToChange, transform.childCount);

        for (int i = 0; i < numberOfCoinsToChange; i++)
        {
            Transform coin = transform.GetChild(childIndices[i]);
            coin.GetComponent<CoinCollectible>().worth = newWorth;
            coin.GetComponent<SpriteRenderer>().color = newColor;
            coin.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
	}

    // Robert Floyd
    int[] RandomSample(int numberOfSamples, int populationSize)
    {
        int[] samples = new int[numberOfSamples];

        for (int i = populationSize - numberOfSamples; i < populationSize; i++)
        {
            int sample = Random.Range(0, i);
            // New element
            if (System.Array.IndexOf(samples, sample) == -1)
            {
                samples[i - populationSize + numberOfSamples] = sample;
            }
            // Repeated element
            else
            {
                samples[i - populationSize + numberOfSamples] = i;
            }
        }

        return samples;
    }
}
