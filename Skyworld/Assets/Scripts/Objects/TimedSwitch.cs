using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSwitch : Switch {

    [SerializeField]
    protected int time;
    protected bool timerOn = false;
    protected float timeElapsed = 0f;

    protected virtual void Update()
    {
        if (timerOn)
        {
            if (timeElapsed >= time)
            {
                Reset();
            }
            timeElapsed += Time.deltaTime;
        }
    }

    protected override IEnumerator Activate(AudioSource clipSource)
    {
        yield return StartCoroutine(base.Activate(clipSource));
        SetMusic(true);
        StartWaiting();
    }

    protected virtual void StartWaiting()
    {
        timeElapsed = 0f;
        TimerManager.Instance.StartTimer(time);
        timerOn = true;
    }

    // Turn off switch if in activated state when leaving zone
    protected override void OnDisable()
    {
        if (!gameObject.activeInHierarchy && gameObject.activeSelf && !GetComponent<CircleCollider2D>().enabled)
        {
            Reset();
        }
    }

    protected virtual void Reset()
    {
        timerOn = false;
        SetMusic(false);
        SetObjects(false);
        SetSwitch(false);
    }
}
