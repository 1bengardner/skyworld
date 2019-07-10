using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class Typer : MonoBehaviour {
    
    public float startDelay;
    public float typeDelay;

    Text t;
    float secondsUntilNextSound;
    bool finishImmediately;
    const float ReadyTime = -1f;

    void Awake()
    {
        t = GetComponent<Text>();
        secondsUntilNextSound = 0f;
    }

    public IEnumerator TypeIn(string dialogue, AudioClip typeSound, AudioSource audioSource, NPCDialogue.Emotion emotion = NPCDialogue.Emotion.Neutral)
    {
        finishImmediately = false;
        float baseDelay = emotion == NPCDialogue.Emotion.Panicked ? typeDelay * 1.2f : typeDelay;

        t.text = "";
        if (audioSource != null)
        {
            audioSource.clip = typeSound;
        }

        yield return new WaitForSeconds(startDelay);

        secondsUntilNextSound = 0f;

        for (int currentLetter = 0
            ; currentLetter < dialogue.Length
            ; currentLetter++)
        {
            if (finishImmediately)
            {
                currentLetter = dialogue.Length - 1;
                finishImmediately = false;
            }

            t.text = dialogue.Substring(0, currentLetter + 1);
            float delay = baseDelay;

            // Pause briefly for sentence-slowing punctuation
            if (",;-".Contains(dialogue[currentLetter].ToString()))
            {
                delay *= 5f;
            }
            // Pause longer for numbers or if a sentence was just completed
            else if (".!?1234567890".Contains(dialogue[currentLetter].ToString()))
            {
                delay *= 10f;
            }

            if (typeSound != null)
            {
                // Play speech sound on next letter if the letter starts a new word
                if (" -".Contains(dialogue[currentLetter].ToString()))
                {
                    secondsUntilNextSound = 0f;
                }
                // Play speech sound now if current character is a number
                else if ("1234567890".Contains(dialogue[currentLetter].ToString()))
                {
                    secondsUntilNextSound = ReadyTime;
                }
                else
                {
                    secondsUntilNextSound -= delay;
                }

                if (secondsUntilNextSound < 0f && !" ,.;:!?()-".Contains(dialogue[currentLetter].ToString()))
                {
                    audioSource.pitch = 1f + (dialogue[currentLetter] % 100 - 50) / 1000f;
                    audioSource.Play();
                    secondsUntilNextSound = typeSound.length;
                }
            }
            yield return new WaitForSeconds(delay);
        }
        DialogueManager.Instance.isTyping = false;
    }

    public void Finish()
    {
        finishImmediately = true;
    }
}
