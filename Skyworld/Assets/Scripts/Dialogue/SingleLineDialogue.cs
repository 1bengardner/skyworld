using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A simple script for NPCs with only only line of dialogue
public class SingleLineDialogue : NPCDialogue {

    [SerializeField] protected TextAsset dialogue;
    protected new TextAsset dialogues;

    void Update()
    {
        if (Input.GetButtonDown("Talk") && canBeTalkedTo)
        {
            if (!talking)
            {
                Talk(dialogue.text);
            }
            else
            {
                StopTalking();
            }
        }
    }
    
    protected override void LatterEncounter(Collider2D player)
    {
        // Empty
    }
}
