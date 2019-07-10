using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItems : MonoBehaviour {

    [SerializeField] PlayerCollecting playerCollecting;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] AudioClip itemClip;
    [SerializeField] AudioClip keyClip;
    
    void Awake ()
    {
        playerCollecting.OnAddOrRemoveItem += (add, item) => UpdateItems();
        playerCollecting.OnAddOrRemoveItem += (add, item) => { if (add && item != null) GetComponent<AudioSource>().PlayOneShot(item.isKey ? keyClip : itemClip); };
    }
	
	void UpdateItems() {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < playerCollecting.items.Count; i++)
        {
            GameObject itemIcon = Instantiate(itemPrefab);
            itemIcon.GetComponent<Image>().sprite = playerCollecting.items[i].hudSprite;
            itemIcon.transform.SetParent(transform, false);
            itemIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * 50f, 0f);
        }
	}
}
