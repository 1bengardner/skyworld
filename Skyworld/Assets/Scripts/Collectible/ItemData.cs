using UnityEngine;

[System.Serializable]
public class ItemData : System.IEquatable<ItemData>
{
    public ItemData(string id, bool isKey)
    {
        this.id = id;
        this.isKey = isKey;
    }
    public bool isKey = false;
    public string id;
    public Sprite hudSprite {
        get { return SpriteTable.Instance.Get(id); }
    }

    public bool Equals(ItemData other)
    {
        return id == other.id;
    }

    public bool Equals(ItemCollectible other)
    {
        return id == other.data.id;
    }

    public static explicit operator ItemData(ItemCollectible v)
    {
        return v.data;
    }
}
