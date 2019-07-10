using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseSliderJointMotor : MonoBehaviour {

    [SerializeField]
    bool stopOnArrival = false;

    public Collider2D platform;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == platform)
        {
            JointMotor2D newMotor = other.GetComponent<SliderJoint2D>().motor;
            if (stopOnArrival)
            {
                newMotor.motorSpeed = 0;
            }
            else
            {
                newMotor.motorSpeed = -newMotor.motorSpeed;
            }
            other.GetComponent<SliderJoint2D>().motor = newMotor;
        }
    }
}
