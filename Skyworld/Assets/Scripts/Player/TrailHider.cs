using UnityEngine;

public class TrailHider : MonoBehaviour {

    PlayerHealth playerHealth;
    TrailRenderer trail;
    
	void Start () {
        playerHealth = GetComponentInParent<PlayerHealth>();
        trail = GetComponent<TrailRenderer>();
        playerHealth.OnDie += Hide;
        playerHealth.OnRespawn += Show;
	}

    void Hide()
    {
        trail.enabled = false;
    }

    void Show()
    {
        trail.enabled = true;
    }
}
