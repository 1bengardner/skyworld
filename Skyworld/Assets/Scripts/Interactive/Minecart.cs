using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecart : Mount {

    [SerializeField] WheelJoint2D[] wheels;
    [SerializeField]
    AudioClip startSound;
    bool mounted = false;

    override protected void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.tag == "Player")
        {
            mounted = true;
        }
    }

    override protected void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.tag == "Player")
        {
            mounted = false;
        }
    }

    void SetMotors(bool active)
    {
        foreach (WheelJoint2D wheel in wheels)
        {
            wheel.useMotor = active;
        }
    }

    void Update()
    {
        if (mounted && Input.GetButtonDown("Interact"))
        {
            GetComponent<AudioSource>().PlayOneShot(startSound);
        }
        else if (!mounted || Input.GetButtonUp("Interact"))
        {
            GetComponent<AudioSource>().Stop();
        }
        if (mounted && Input.GetButton("Interact"))
        {
            SetMotors(true);
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            SetMotors(false);
        }
    }
}
