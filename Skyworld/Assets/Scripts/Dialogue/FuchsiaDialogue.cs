using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuchsiaDialogue : NPCDialogue
{
    [SerializeField]
    TextAsset[] helpDialogues;
    [SerializeField]
    ItemCollectible spacePart;
    bool gotSpacePart = false;
    Dictionary<int, bool> helpSeen;

    void Update()
    {
        if (Input.GetButtonDown("Talk") && canBeTalkedTo)
        {
            if (!talking)
            {
                LatterEncounter(player);
                if (!talking)
                {
                    string dialogue;
                    if (gotSpacePart)
                    {
                        dialogue = dialogues[1].text;
                    }
                    else
                    {
                        int i = Random.Range(1, helpDialogues.Length);
                        dialogue = helpDialogues[i].text;
                        TextAsset temp = helpDialogues[i];
                        helpDialogues[i] = helpDialogues[0];
                        helpDialogues[0] = temp;
                    }
                    Talk(dialogue);
                }
            }
            else
            {
                StopTalking();
            }
        }
    }

    protected override void LatterEncounter(Collider2D player)
    {
        // Introduction complete & has space part
        if (player.GetComponent<PlayerCollecting>().HasItem(spacePart))
        {
            player.GetComponent<PlayerCollecting>().RemoveItem(spacePart);
            GameManager.Instance.partsFound++;
            gotSpacePart = true;
            string dialogue = dialogues[1].text;
            Talk(dialogue);
            ZoomIn();
            player.GetComponent<AudioSource>().PlayOneShot(progressSound);
        }
    }
}
