using System.Collections;
using UnityEngine;

public class Lever : Switch, ISavable {
    
    protected PlayerCollecting puller;
    protected bool pulled = false;

    protected virtual void Update()
    {
        if (Input.GetButtonDown("Interact") && !pulled && puller != null && (GameManager.Instance == null || !GameManager.Instance.paused))
        {
            Pull(puller.GetComponent<AudioSource>());
        }
    }

    protected virtual void Pull(AudioSource clipSource)
    {
        pulled = true;
        StartCoroutine(Activate(clipSource));
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            puller = other.GetComponent<PlayerCollecting>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        puller = null;
    }

    StateRecord ISavable.GetRecord()
    {
        LeverRecord record = (LeverRecord)RecordFactory.Get(this);
        record.pulled = pulled;
        return record;
    }

    void ISavable.SetData(StateRecord loaded)
    {
        LeverRecord record = (LeverRecord)loaded;
        if (pulled != record.pulled)
        {
            SetSwitch(true);
            SetObjects(true);
        }
        pulled = record.pulled;
    }
}
