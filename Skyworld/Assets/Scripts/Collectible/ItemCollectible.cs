using UnityEngine;

public class ItemCollectible : MonoBehaviour, ISavable
{
    [SerializeField] bool isKey = false;
    [SerializeField] string id;
    [SerializeField] AudioClip collectClip;
    public ItemData data {
        get
        {
            if (_data == null || string.IsNullOrEmpty(_data.id))
            {
                Debug.Log("Initializing ItemData for ItemCollectible \"" + id +"\".");
                data = new ItemData(id, isKey);
            }
            return _data;
        }
        private set
        {
            _data = value;
        }
    }
    ItemData _data;

    void Start()
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning("Warning: There is an ItemCollectible in the scene with no ID.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // TODO: Add individual item sounds (e.g. metal clang for Pan item)
            //other.GetComponent<AudioSource>().PlayOneShot(collectClip);
            other.GetComponent<PlayerCollecting>().CollectItem(data);
            gameObject.SetActive(false);
        }
    }

    StateRecord ISavable.GetRecord()
    {
        ItemCollectibleRecord record = (ItemCollectibleRecord)RecordFactory.Get(this);
        record.active = gameObject.activeSelf;
        return record;
    }

    void ISavable.SetData(StateRecord loaded)
    {
        ItemCollectibleRecord record = (ItemCollectibleRecord)loaded;
        gameObject.SetActive(record.active);
    }
}
