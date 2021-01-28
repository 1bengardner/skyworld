using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FheliciaDialogue : NPCDialogue
{
    [SerializeField]
    ItemCollectible itemReward;
    [SerializeField]
    ItemCollectible itemNeeded;
    [SerializeField]
    AudioClip celebrateSound;
    bool gotItem = false;

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
                    if (gotItem)
                    {
                        dialogue = dialogues[2].text;
                    }
                    else
                    {
                        dialogue = dialogues[0].text;
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
        // Introduction complete & has item
        if (player.GetComponent<PlayerCollecting>().HasItem(itemNeeded))
        {
            Congratulate(celebrateSound);
            string dialogue = dialogues[1].text;
            Talk(dialogue);
            ZoomIn();
            player.GetComponent<PlayerCollecting>().RemoveItem(itemNeeded);
            player.GetComponent<PlayerCollecting>().CollectItem(itemReward);
            gotItem = true;
            player.GetComponent<AudioSource>().PlayOneShot(progressSound);
        }
    }
}
