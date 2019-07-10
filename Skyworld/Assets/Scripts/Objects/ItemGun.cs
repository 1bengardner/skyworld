using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGun : MonoBehaviour
{
    float delay = 1f;
    float timeElapsed = 1f;

    void OnTriggerStay2D(Collider2D other)
    {
        ItemCollectible item = other.GetComponent<ItemCollectible>();
        if (item == null)
            return;
        foreach (GameManager.SpecialItem si in GameManager.Instance.specialItems)
        {
            if (((ItemData)item).Equals((ItemData)si.item) && timeElapsed > delay)
            {
                Fire();
                timeElapsed = 0f;
            }
        }
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
    }

    void Fire()
    {
        GetComponent<Animator>().SetTrigger("Fire");
    }
}
