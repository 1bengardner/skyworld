using UnityEngine;
using System.Collections.Generic;

public class StateManager : MonoBehaviour
{
    [SerializeField] ObjectState savedPlayer;
    [SerializeField] List<ObjectState> savedObjects;

    // Singleton
    public static StateManager Instance
    {
        get
        {
            instance = instance ?? FindObjectOfType<StateManager>();
            return instance;
        }
    }
    private static StateManager instance = null;

    void Awake()
    {
        if (Instance != this)
        {
            Debug.LogWarning("Destroying duplicate StateManager.");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (savedPlayer == null)
        {
            Debug.LogWarning("GameState: ObjectState savedPlayer is uninitialized.");
        }
        if (savedObjects == null)
        {
            Debug.LogWarning("GameState: List<ObjectState> savedObjects is uninitialized.");
            savedObjects = new List<ObjectState>();
        }
        for (int i = 0; i < savedObjects.Count; i++)
        {
            savedObjects[i].InitializeID();
        }
    }
    
    void SetState(GameState newState)
    {
        savedPlayer.SetRecord(newState.playerRecord);
        for (int i = 0; i < savedObjects.Count; i++)
        {
            StateRecord record = savedObjects[i].GetRecord();
            if (record.id == null)
            {
                Debug.LogError("SetState: Scene saved object record ID cannot be null!");
                continue;
            }
            if (!newState.records.ContainsKey((uint)record.id))
            {
                Debug.LogWarning("SetState: Loaded state is missing data for object ID #" + record.id);
                continue;
            }
            savedObjects[i].SetRecord(newState.records[(uint)record.id]);
        }
    }

    GameState GetState()
    {
        GameState state = new GameState();

        state.playerRecord = savedPlayer.GetRecord();
        for (int i = 0; i < savedObjects.Count; i++)
        {
            StateRecord record = savedObjects[i].GetRecord();
            if (record.id == null)
            {
                Debug.LogError("GetState: Record ID cannot be null!");
                continue;
            }
            state.records[(uint)record.id] = record;
        }
        return state;
    }

    public void Save()
    {
        StatePersistence.Save(GetState(), GameManager.Instance.fileName);
    }

    public void Load()
    {
        SetState(StatePersistence.Load(GameManager.Instance.fileName));
    }
}
