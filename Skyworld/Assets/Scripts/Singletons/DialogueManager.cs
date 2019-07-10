using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    
    // Singleton
    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogueManager>();
            }
            return instance;
        }
    }
    private static DialogueManager instance = null;
    public bool isTyping;
    [SerializeField]
    Animator anim;
    [SerializeField]
    Text nameText;
    [SerializeField]
    Typer typer;
    IEnumerator coroutine;
    public bool isDisplaying
    {
        get { return _isDisplaying; }
        private set { _isDisplaying = value; }
    }
    bool _isDisplaying;

    //void Start()
    //{
    //    // Preload UI to prevent waiting in game
    //    bubbleDisplay.gameObject.SetActive(true);
    //    bubbleDisplay.gameObject.SetActive(false);
    //}

    public void StartDialogue(string speaker, string dialogue, AudioClip dialogueSound = null, AudioSource dialogueSource = null, NPCDialogue.Emotion emotion = NPCDialogue.Emotion.Neutral)
    {
        _isDisplaying = true;
        isTyping = true;
        nameText.text = speaker;
        anim.SetTrigger("Fade In");
        coroutine = typer.TypeIn(dialogue, dialogueSound, dialogueSource, emotion);
        StartCoroutine(coroutine);
    }

    public void FinishDialogue()
    {
        typer.Finish();
        coroutine.MoveNext();
    }
	
	public void EndDialogue()
    {
        _isDisplaying = false;
        isTyping = false;
        StopCoroutine(coroutine);
        anim.SetTrigger("Fade Out");
        //nameText.text = "";
    }

    public void SwitchDialogue(string speaker, string dialogue, AudioClip dialogueSound, AudioSource dialogueSource, NPCDialogue.Emotion emotion)
    {
        if (_isDisplaying)
        {
            StopCoroutine(coroutine);
            nameText.text = speaker;
            coroutine = typer.TypeIn(dialogue, dialogueSound, dialogueSource, emotion);
            StartCoroutine(coroutine);
        }
    }
}
