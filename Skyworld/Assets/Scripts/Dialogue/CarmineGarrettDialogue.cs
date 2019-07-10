using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarmineGarrettDialogue : NPCDialogue
{
    [SerializeField]
    TextAsset[] specialItemDialogues;
    [SerializeField]
    TextAsset[] specialItemInstructions;
    [SerializeField]
    PlayerCollecting playerCollecting;
    [SerializeField]
    int purchasePrice;
    bool playerHasEnoughMoney;
    int previousDialogue = 0;

    void Awake()
    {
        playerCollecting.OnAddOrRemoveItem += (add, item) => { if (add && item != null) GiveItemInformation(item); } ;
    }

    void GiveItemInformation(ItemData item)
    {
        for (int i = 0; i < GameManager.Instance.specialItems.Length; i++)
        {
            if (item.Equals((ItemData)GameManager.Instance.specialItems[i].item))
            {
                string dialogue = specialItemDialogues[i].text;
                Talk(dialogue);
                if (!zoomed)
                    ZoomIn();
                canBeTalkedTo = true;
                endDialogueHandler = () => canBeTalkedTo = false;

                GameObject uiInfo = GameObject.FindWithTag("Info UI");
                if (uiInfo != null)
                {
                    uiInfo.GetComponent<UIInfo>().infoString = specialItemInstructions[i].text;
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Talk") && canBeTalkedTo)
        {
            if (!talking)
            {
                string dialogue;
                if (playerHasEnoughMoney)
                {
                    dialogue = dialogues[2].text;
                }
                else
                {
                    dialogue = dialogues[previousDialogue % 2].text;
                    previousDialogue++;
                }
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
        // Introduction complete & have enough coins
        if (player.GetComponent<PlayerCollecting>().money >= purchasePrice)
        {
            if (!talking)
            {
                string dialogue = dialogues[2].text;
                Talk(dialogue);
                ZoomIn();
            }
            playerHasEnoughMoney = true;
        }
        else
        {
            playerHasEnoughMoney = false;
        }
    }
}
