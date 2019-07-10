using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICoinText : MonoBehaviour {

    [SerializeField] PlayerCollecting playerCollecting;
    [SerializeField] Text coinText;
    Animator anim;
    
	void Awake ()
    {
        anim = GetComponent<Animator>();

        playerCollecting.OnAddOrRemoveMoney += UpdateCoinText;
        playerCollecting.OnAddOrRemoveMoney += Jump;

        UpdateCoinText();
    }

    void UpdateCoinText()
    {
        coinText.text = playerCollecting.money.ToString();
    }

    void Jump()
    {
        anim.SetTrigger("Jump");
    }
}
