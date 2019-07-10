using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActivatedObject : MonoBehaviour {

    [SerializeField]
    PlayerCollecting playerCollecting;
    [SerializeField]
    ItemCollectible activationItem;
    [SerializeField]
    GameObject objectToActivate;
    
    void Start () {
        playerCollecting.OnAddOrRemoveItem += (add, item) => { if (item != null && item.Equals((ItemData)activationItem)) objectToActivate.SetActive(add); };
	}
}
