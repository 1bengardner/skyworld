using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour, ISavable {

    [SerializeField] Sprite emptyBox;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] AudioClip hitClip;
    bool opened;

    StateRecord ISavable.GetRecord()
    {
        ItemBoxRecord record = (ItemBoxRecord)RecordFactory.Get(this);
        record.opened = opened;
        return record;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !opened)
        {
            opened = true;
            GetComponent<SpriteRenderer>().sprite = emptyBox;
            other.GetComponent<AudioSource>().PlayOneShot(hitClip);
            GameObject item = Instantiate(itemPrefab, transform.parent);
            item.transform.position = transform.position + transform.up;
            item.GetComponent<Rigidbody2D>().AddForce(150f * transform.up);
            item.GetComponent<Rigidbody2D>().AddTorque(30f);
            GetComponent<AudioSource>().enabled = false;
        }
    }

    void ISavable.SetData(StateRecord loaded)
    {
        ItemBoxRecord record = (ItemBoxRecord)loaded;
        opened = record.opened;
        if (opened)
        {
            GetComponent<SpriteRenderer>().sprite = emptyBox;
            GetComponent<AudioSource>().enabled = false;
        }
    }
}
