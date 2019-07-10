using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Display info one time upon entering trigger
[RequireComponent(typeof(Collider2D))]
public class InfoDisplayTrigger : MonoBehaviour, ISavable {
    [SerializeField] string infoString;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetComponent<Collider2D>().enabled = false;
            GameObject uiInfo = GameObject.FindWithTag("Info UI");
            if (uiInfo != null)
            {
                uiInfo.GetComponent<UIInfo>().infoString = infoString;
            }
        }
    }

    StateRecord ISavable.GetRecord()
    {
        InfoDisplayTriggerRecord record = (InfoDisplayTriggerRecord)RecordFactory.Get(this);
        record.triggered = !GetComponent<Collider2D>().enabled;
        return record;
    }

    void ISavable.SetData(StateRecord loaded)
    {
        InfoDisplayTriggerRecord record = (InfoDisplayTriggerRecord)loaded;
        GetComponent<Collider2D>().enabled = !record.triggered;
    }
}
