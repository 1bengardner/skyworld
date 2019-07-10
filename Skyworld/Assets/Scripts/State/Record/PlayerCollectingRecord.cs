using System.Collections.Generic;

[System.Serializable]
public class PlayerCollectingRecord : StateRecord {
    public int money;
    public List<ItemData> items;
}
