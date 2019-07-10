using System.Collections.Generic;

[System.Serializable]
public class GameState {
    public Dictionary<uint, StateRecord> records = new Dictionary<uint, StateRecord>();
    public StateRecord playerRecord;
}