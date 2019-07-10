using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDialogue : NPCDialogue, ISavable
{
    /*
        dialogues[0]: intro
        dialogues[1]: complete
        dialogues[2]: aftermath
    */
    [SerializeField]
    ItemCollectible rewardItem;
    [SerializeField]
    ItemCollectible[] requiredItems;
    bool rewardGiven = false;

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
                    if (rewardGiven)
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
        bool playerHasItems = true;
        foreach (ItemCollectible requiredItem in requiredItems)
        {
            if (!player.GetComponent<PlayerCollecting>().HasItem(requiredItem))
            {
                playerHasItems = false;
            }
        }
        // Introduction complete & found items
        if (playerHasItems)
        {
            string dialogue = dialogues[1].text;
            Talk(dialogue);
            ZoomIn();
            foreach (ItemCollectible requiredItem in requiredItems)
            {
                player.GetComponent<PlayerCollecting>().RemoveItem(requiredItem);
            }
            player.GetComponent<PlayerCollecting>().CollectItem(rewardItem);
            rewardGiven = true;
            player.GetComponent<AudioSource>().PlayOneShot(progressSound);
        }
    }

    StateRecord ISavable.GetRecord()
    {
        QuestRecord record = (QuestRecord)RecordFactory.Get(this);
        record.rewardGiven = rewardGiven;
        record.hasBeenTalkedTo = hasBeenTalkedTo;
        return record;
    }

    void ISavable.SetData(StateRecord loaded)
    {
        QuestRecord record = (QuestRecord)loaded;
        rewardGiven = record.rewardGiven;
        hasBeenTalkedTo = record.hasBeenTalkedTo;
    }
}
