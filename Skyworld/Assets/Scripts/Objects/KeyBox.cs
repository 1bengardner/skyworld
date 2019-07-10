using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBox : MonoBehaviour, ISavable {

    [SerializeField]
    ItemCollectible unlockingItem;
    [SerializeField]
    AudioClip unlockClip;
    [SerializeField]
    AudioClip lockedClip;
    bool unlocked = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!unlocked && other.tag == "Player")
        {
            if (other.GetComponent<PlayerCollecting>().HasItem(unlockingItem))
            {
                unlocked = true;
                Camera.main.GetComponent<Camera2DFollow>().PushTarget(transform);
                GetComponent<Animator>().SetTrigger("Unlock");
                other.GetComponent<AudioSource>().PlayOneShot(unlockClip);
            }
            else
            {
                other.GetComponent<AudioSource>().PlayOneShot(lockedClip);
            }
        }
    }

    void Destroy()
    {
        Camera.main.GetComponent<Camera2DFollow>().PopTarget();
        gameObject.SetActive(false);
    }

    StateRecord ISavable.GetRecord()
    {
        KeyBoxRecord record = (KeyBoxRecord)RecordFactory.Get(this);
        record.unlocked = unlocked;
        return record;
    }

    void ISavable.SetData(StateRecord loaded)
    {
        KeyBoxRecord record = (KeyBoxRecord)loaded;
        unlocked = record.unlocked;
        if (unlocked)
        {
            GetComponent<Animator>().SetTrigger("Unlock");
        }
    }
}
