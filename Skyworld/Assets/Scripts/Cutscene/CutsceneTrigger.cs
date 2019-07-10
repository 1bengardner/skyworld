using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Cutscene))]
public class CutsceneTrigger : MonoBehaviour, ISavable {
    
    Cutscene cutscene;

	// Use this for initialization
	void Start ()
    {
        cutscene = GetComponent<Cutscene>();
        if (cutscene == null)
        {
            Debug.LogError("CutsceneTrigger is missing a Cutscene.");
        }
	}
	
	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && cutscene.playable)
        {
            cutscene.Play();
        }
    }

    StateRecord ISavable.GetRecord()
    {
        CutsceneTriggerRecord record = (CutsceneTriggerRecord)RecordFactory.Get(this);
        record.triggered = !cutscene.playable;
        return record;
    }

    void ISavable.SetData(StateRecord loaded)
    {
        CutsceneTriggerRecord record = (CutsceneTriggerRecord)loaded;
        cutscene.playable = !record.triggered;
    }
}
