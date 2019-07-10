using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(ISavable))]
public class ObjectState : MonoBehaviour {
    uint? id;

    // Write state to record
    public StateRecord GetRecord()
    {
        StateRecord record = GetComponent<ISavable>().GetRecord();
        
        record.id = id;

        return record;
    }

    // Overwrite current object state with record
    public void SetRecord(StateRecord record)
    {
        GetComponent<ISavable>().SetData(record);
    }

    public void InitializeID()
    {
        if (id == null)
        {
            id = (uint)(1000000000 + 40000 * transform.position.x + 40 * transform.position.y);
        }
        else
        {
            Debug.LogWarning("ObjectState ID is already initialized.");
        }
    }
}
