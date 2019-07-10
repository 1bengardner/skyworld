using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitToClick : MonoBehaviour {
	
	void Update () {
        if (Input.GetButtonDown("Submit") && GetComponent<Button>().interactable)
        {
            GetComponent<Button>().onClick.Invoke();
        }
    }
}
