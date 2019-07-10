using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeEffect : MonoBehaviour {

    public bool on = false;
    public float magnitude; // Set this before turning on
    [Range(0,1)]
    public float falloff;
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }
	
	void Update () {
        const float falloffMultiplier = 20f;
        if (on)
        {
            if (magnitude <= 0f)
            {
                Debug.LogWarning("ShakeEffect is trying to shake with magnitude <= 0.");
            }
            cam.transform.localPosition += (Vector3) (Random.insideUnitCircle * magnitude);
            magnitude *= Mathf.Clamp01(1 - falloff * Time.deltaTime * falloffMultiplier);
        }
	}
}
