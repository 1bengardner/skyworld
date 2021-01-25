using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmberJackDialogue : NPCDialogue, ISavable
{
    public enum Area { Grass, Snow, Desert };
    public Area zone;
    [SerializeField] ItemCollectible axe;
    [SerializeField] ItemCollectible flashlight;
    [SerializeField] GameObject rollingPinPrefab;
    [SerializeField] GameObject pencilPrefab;
    [SerializeField] Collider2D forestWarp;
    [SerializeField] Collider2D dungeonWarp;
    [SerializeField] Animator tree;
    bool gotAxe = false;
    bool gotFlashlight = false;
    bool gavePencil = false;

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
                    if (gotAxe || gotFlashlight || gavePencil)
                    {
                        dialogue = dialogues[1].text;
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

    protected override void FirstEncounter(Collider2D other)
    {
        if (zone == Area.Desert)
        {
            LatterEncounter(other);
        }
        else
        {
            base.FirstEncounter(other);
        }
    }

    protected override void LatterEncounter(Collider2D player)
    {
        // Introduction complete & axe found
        if (zone == Area.Grass && player.GetComponent<PlayerCollecting>().HasItem(axe))
        {
            Congratulate();
            string dialogue = dialogues[1].text;
            Talk(dialogue);
            ZoomIn();
            player.GetComponent<PlayerCollecting>().RemoveItem(axe);
            gotAxe = true;
            endDialogueHandler = GotAxe;
            player.GetComponent<AudioSource>().PlayOneShot(progressSound);
        }
        else if (zone == Area.Snow && player.GetComponent<PlayerCollecting>().HasItem(flashlight))
        {
            Congratulate();
            string dialogue = dialogues[1].text;
            Talk(dialogue);
            ZoomIn();
            player.GetComponent<PlayerCollecting>().RemoveItem(flashlight);
            gotFlashlight = true;
            endDialogueHandler = GotFlashlight;
            player.GetComponent<AudioSource>().PlayOneShot(progressSound);
        }
        else if (zone == Area.Desert && !gavePencil)
        {
            Congratulate();
            string dialogue = dialogues[0].text;
            Talk(dialogue);
            ZoomIn();
            gavePencil = true;
            endDialogueHandler = GotPencil;
            player.GetComponent<AudioSource>().PlayOneShot(progressSound);
        }
    }

    void GotAxe()
    {
        canBeTalkedTo = false;
        anim.SetTrigger("Got Axe");
    }

    void CutTree()
    {
        tree.SetTrigger("Cut Tree");
    }

    void OpenForest()
    {
        forestWarp.enabled = true;
        //HideChildren();
    }

    void GotFlashlight()
    {
        canBeTalkedTo = false;
        dungeonWarp.gameObject.SetActive(true);
        dungeonWarp.enabled = false;
        anim.SetTrigger("Got Flashlight");
        GameObject item = Instantiate(rollingPinPrefab, transform.parent);
        item.transform.position = transform.position + Vector3.up;
        item.GetComponent<Rigidbody2D>().AddForce(150f * Vector2.up);
        item.GetComponent<Rigidbody2D>().AddTorque(30f);
    }

    void GotPencil()
    {
        anim.SetTrigger("Get Pencil");
        GameObject item = Instantiate(pencilPrefab, transform.parent);
        item.transform.position = transform.position + Vector3.up;
        item.GetComponent<Rigidbody2D>().AddForce(50f * Vector2.up);
        item.GetComponent<Rigidbody2D>().AddTorque(30f);
    }

    void OpenDungeonDoor()
    {
        dungeonWarp.enabled = true;
        //HideChildren();
    }

    void HideChildren()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
    }

    StateRecord ISavable.GetRecord()
    {
        UmberJackDialogueRecord record = (UmberJackDialogueRecord)RecordFactory.Get(this);
        record.gotAxe = gotAxe;
        record.gotFlashlight = gotFlashlight;
        record.gavePencil = gavePencil;
        return record;
    }

    void ISavable.SetData(StateRecord loaded)
    {
        UmberJackDialogueRecord record = (UmberJackDialogueRecord)loaded;
        gotAxe = record.gotAxe;
        gotFlashlight = record.gotFlashlight;
        gavePencil = record.gavePencil;
    }
}
