using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollecting : MonoBehaviour, ISavable {

    public delegate void MoneyAction();
    public delegate void ItemAction(bool add, ItemData item);
    public event MoneyAction OnAddOrRemoveMoney;
    public event ItemAction OnAddOrRemoveItem;

    public List<ItemData> items { get; private set; }
    public int money { get; private set; }
    [SerializeField] int startMoney;

    void Start()
    {
        items = new List<ItemData>();
        money = startMoney;
        if (OnAddOrRemoveMoney != null)
        {
            OnAddOrRemoveMoney();
        }
    }

    public void CollectItem(ItemCollectible item)
    {
        CollectItem((ItemData)item);
    }

    public void CollectItem(ItemData item)
    {
        items.Add(item);
        // If special, add the item as a child of this GameObject
        for (int i = 0; i < GameManager.Instance.specialItems.Length; i++)
        {
            if (item.Equals((ItemData)GameManager.Instance.specialItems[i].item))
            {
                Instantiate(GameManager.Instance.specialItems[i].itemPrefab, transform, false);
            }
        }
        if (OnAddOrRemoveItem != null)
        {
            OnAddOrRemoveItem(true, item);
        }

    }
	
	public void CollectMoney(int quantity)
    {
        money += quantity;
        if (OnAddOrRemoveMoney != null)
        {
            OnAddOrRemoveMoney();
        }
    }

    public void RemoveMoney(int quantity)
    {
        money -= quantity;
        if (OnAddOrRemoveMoney != null)
        {
            OnAddOrRemoveMoney();
        }
    }

    public bool HasItem(ItemCollectible item)
    {
        return HasItem((ItemData)item);
    }

    public bool HasItem(ItemData item)
    {
        return items.Contains(item);
    }

    public void RemoveItem(ItemCollectible item)
    {
        RemoveItem((ItemData)item);
    }
    
    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
        if (OnAddOrRemoveItem != null)
        {
            OnAddOrRemoveItem(false, item);
        }
    }

    StateRecord ISavable.GetRecord()
    {
        PlayerCollectingRecord record = (PlayerCollectingRecord)RecordFactory.Get(this);
        record.money = money;
        record.items = items;
        return record;
    }

    void ISavable.SetData(StateRecord loaded)
    {
        PlayerCollectingRecord record = (PlayerCollectingRecord)loaded;
        money = record.money;
        items = new List<ItemData>(record.items);

        if (OnAddOrRemoveMoney != null)
        {
            OnAddOrRemoveMoney();
        }
        if (OnAddOrRemoveItem != null)
        {
            OnAddOrRemoveItem(true, null);
        }
    }
}
