using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QirkDialogue : NPCDialogue
{
    void Update()
    {
        if (Input.GetButtonDown("Talk") && canBeTalkedTo)
        {
            if (!talking)
            {
                string dialogue = dialogues[1].text;
                Talk(dialogue);
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
