using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwap : MonoBehaviour {

    [System.Serializable]
    public struct SpritesToSwap
    {
        public SpriteRenderer spriteRenderer;
        public Sprite newSprite;
    }
    public SpritesToSwap[] spritesToSwap;

    void Awake()
    {
        for (int i = 0; i < spritesToSwap.Length; i++)
        {
            if (spritesToSwap[i].spriteRenderer == null)
            {
                Debug.LogError("Assign a sprite renderer to sprite " + i + ".");
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #endif
            }
        }
    }

    public void Swap()
    {
        for (int i = 0; i < spritesToSwap.Length; i++)
        {
            Sprite temp = spritesToSwap[i].spriteRenderer.sprite;
            spritesToSwap[i].spriteRenderer.sprite = spritesToSwap[i].newSprite;
            spritesToSwap[i].newSprite = temp;
        }
    }
}
