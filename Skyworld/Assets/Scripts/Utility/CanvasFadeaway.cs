using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFadeaway : MonoBehaviour {

    [SerializeField]
    Transform fadeBasedOnDistanceFrom;
    CanvasGroup cg;
    const float distanceForFullOpacity = 0.87f;
    const float closenessBeforeFading = 4f;
	
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
    }

	// Update is called once per frame
	void Update () {
        cg.alpha = Mathf.Lerp(1f, 0f, Mathf.Clamp((Vector3.Magnitude(transform.position - fadeBasedOnDistanceFrom.position) - distanceForFullOpacity) * closenessBeforeFading, 0f, 1f));
	}
}
