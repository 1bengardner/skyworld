using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public abstract class NPCDialogue : MonoBehaviour {

    public enum Emotion { Neutral, Happy, Angry, Panicked, Inquisitive };

    protected string speaker;
    [SerializeField]
    protected TextAsset[] dialogues;
    [SerializeField]
    protected AudioClip dialogueSound;
    protected AudioClip progressSound;
    protected bool hasBeenTalkedTo = false;
    protected bool canBeTalkedTo;
    protected bool talking;
    protected bool zoomed;
    protected delegate void DialogueAction();
    protected DialogueAction endDialogueHandler;    // Add functionality to ending a dialogue through this delegate
    protected AudioSource dialogueSource;
    protected Animator anim;
    protected Collider2D player;
    
    private bool celebrating;

    void Start()
    {
        speaker = name;
        dialogueSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        progressSound = GameManager.Instance.progressSound;
    }

    Emotion PreprocessDialogue(ref string dialogue)
    {
        Emotion emotion = Emotion.Neutral;

        // Get first tag from dialogue
        Match firstMatch = Regex.Match(dialogue, "\\[.+\\]");
        string emotionString = firstMatch.ToString().Trim("[]".ToCharArray());

        // See if first tag is an Emotion
        if (System.Enum.IsDefined(typeof(Emotion), emotionString))
        {
            // Store emotion and remove emotion tag from dialogue
            emotion = (Emotion)System.Enum.Parse(typeof(Emotion), emotionString);
            dialogue = dialogue.Remove(firstMatch.Index, firstMatch.Length).Trim();
        }

        // Convert all remaining tags
        dialogue = StringTagConverter.ConvertString(dialogue);

        return emotion;
    }

    protected void Talk(string dialogue)
    {
        bool switchDialogue = talking;
        talking = true;
        Emotion emotion = PreprocessDialogue(ref dialogue);
        anim.SetTrigger(emotion.ToString());
        if (switchDialogue)
        {
            DialogueManager.Instance.SwitchDialogue(speaker, dialogue, dialogueSound, dialogueSource, emotion);
        }
        else
        {
            DialogueManager.Instance.StartDialogue(speaker, dialogue, dialogueSound, dialogueSource, emotion);
        }
    }

    protected void StopTalking()
    {
        if (DialogueManager.Instance.isTyping && canBeTalkedTo)
        {
            DialogueManager.Instance.FinishDialogue();
        }
        else
        {
            if (zoomed)
            {
                ZoomOut();
            }
            talking = false;
            Emotion emotion = Emotion.Neutral;
            anim.SetTrigger(emotion.ToString());
            DialogueManager.Instance.EndDialogue();
            
            if (endDialogueHandler != null)
            {
                endDialogueHandler();
                endDialogueHandler = null;
            }
        }
        if (celebrating)
        {
            celebrating = false;
            GameManager.Instance.backgroundMusic.SwapClip(null);
        }
    }

    protected virtual void FirstEncounter(Collider2D other)
    {
        string dialogue = dialogues[0].text;
        Talk(dialogue);
        ZoomIn();
    }

    protected void Congratulate(AudioClip congratulationSound = null)
    {
        celebrating = true;
        GameManager.Instance.backgroundMusic.SwapClip(congratulationSound ?? GameManager.Instance.backgroundMusic.clipCelebrate);
    }

    // Subclasses may implement their own events when player enters trigger after first time
    protected abstract void LatterEncounter(Collider2D player);

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = other;
            canBeTalkedTo = true;
            if (!hasBeenTalkedTo)
            {
                FirstEncounter(other);
                hasBeenTalkedTo = true;
            }
            else
            {
                LatterEncounter(other);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = null;
            canBeTalkedTo = false;
            if (talking)
            {
                StopTalking();
            }
        }
    }

    void OnDisable()
    {
        canBeTalkedTo = false;
        if (talking)
        {
            StopTalking();
        }
    }

    protected void ZoomIn()
    {
        Camera.main.GetComponent<Camera2DFollow>().PushTarget(transform);
        Camera.main.GetComponent<ZoomEffect>().Activate();
        zoomed = true;
        GameManager.Instance.backgroundMusic.QuietDown(true);
    }

    void ZoomOut()
    {
        Camera.main.GetComponent<Camera2DFollow>().PopTarget();
        Camera.main.GetComponent<ZoomEffect>().Deactivate();
        zoomed = false;
        GameManager.Instance.backgroundMusic.QuietDown(false);
    }
}

