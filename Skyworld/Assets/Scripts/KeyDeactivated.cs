using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class KeyDeactivated : MonoBehaviour {
    [SerializeField] string keyName;
	// Update is called once per frame
	void Update () {
		if (CrossPlatformInputManager.GetButtonDown(keyName) && (GameManager.Instance == null || !GameManager.Instance.paused))
        {
            gameObject.SetActive(false);
        }
	}
}
