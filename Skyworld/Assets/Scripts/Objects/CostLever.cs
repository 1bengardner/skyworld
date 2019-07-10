using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CostLever : Lever, ISavable {

    [SerializeField]
    int requiredCoins;
    [SerializeField]
    AudioClip notEnoughCoins;
    [SerializeField]
    Transform costPanel;

    void Start()
    {
        costPanel.GetComponentInChildren<Text>().text = requiredCoins.ToString();
    }

    protected override void Pull(AudioSource clipSource)
    {
        if (puller.money >= requiredCoins)
        {
            puller.RemoveMoney(requiredCoins);
            costPanel.gameObject.SetActive(false);
            base.Pull(clipSource);
        }
        else
        {
            clipSource.PlayOneShot(notEnoughCoins);
        }
    }
}
