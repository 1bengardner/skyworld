using UnityEngine;

public class Resetter : MonoBehaviour {

    [SerializeField] Transform target;
    [SerializeField] Vector2 minExtents;
    [SerializeField] Vector2 maxExtents;
    [SerializeField] GameObject respawnEffect;

	// Update is called once per frame
	void Update () {
		if (target.localPosition.x < minExtents.x || target.localPosition.x > maxExtents.x || target.localPosition.y < minExtents.y || target.localPosition.y > maxExtents.y)
        {
            target.position = GameManager.Instance.currentZone.transform.position + Vector3.up;
            Instantiate(respawnEffect, GameManager.Instance.currentZone.transform.position, Quaternion.identity);
        }
	}
}
