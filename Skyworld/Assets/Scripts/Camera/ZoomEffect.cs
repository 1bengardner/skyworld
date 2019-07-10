using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomEffect : MonoBehaviour {

    [Range(0.01f, 1f)]
    public float zoomSpeed;
    public float endFov;
    bool on;
    float originalFov;
    float secondsElapsed;
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        originalFov = cam.fieldOfView;
    }

    void Update()
    {
        if (on)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, endFov, Time.deltaTime * zoomSpeed);
        }
        else
        {
            if (secondsElapsed * zoomSpeed <= 1f)
            {
                secondsElapsed += Time.deltaTime;
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, originalFov, secondsElapsed * zoomSpeed);
            }
        }
    }

    public void Activate()
    {
        if (!on)
        {
            on = true;
            GameManager.Instance.paused = true;
        }
    }

    public void Deactivate()
    {
        if (on)
        {
            secondsElapsed = 0f;
            on = false;
            GameManager.Instance.paused = false;
        }
    }
}
