using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMount : Mount {

    IEnumerator routine;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            base.OnTriggerEnter2D(other);
            StartCoroutine("StartMoving");
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            base.OnTriggerExit2D(other);
            StopCoroutine("StartMoving");
        }
    }

    IEnumerator StartMoving()
    {
        const float secondsUntilStartMoving = 1f;
        yield return new WaitForSeconds(secondsUntilStartMoving);
        GetComponent<SliderJoint2D>().useMotor = true;
    }
}
