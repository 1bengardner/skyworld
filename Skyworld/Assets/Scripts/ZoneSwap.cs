using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ZoneSwap : MonoBehaviour
{
    public Zone zoneToShow;
    bool isOperational;
    bool isOpen;
    SpriteSwap spriteSwap;
    AudioSource audioSource;

    void Start()
    {
        spriteSwap = GetComponent<SpriteSwap>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        // Close door behind you
        if (spriteSwap != null && isOpen)
        {
            isOpen = false;
            spriteSwap.Swap();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isOperational = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isOperational = false;
        }
    }

    void Update()
    {
        if (CrossPlatformInputManager.GetAxisRaw("Vertical") == 1f && isOperational && (GameManager.Instance == null || !GameManager.Instance.paused))
        {
            isOperational = false;
            if (spriteSwap != null && !isOpen)
            {
                isOpen = true;
                spriteSwap.Swap();
            }
            audioSource.Play();
            GameManager.Instance.SwapZonesWrapper(zoneToShow);
        }
    }
}