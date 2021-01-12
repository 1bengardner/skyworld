using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDelaySwitch : TimedSwitch
{
    [SerializeField]
    float secondsBetweenActivations;
    float cleanUpTime;
    bool cleaningUp;

    void Start()
    {
        cleanUpTime = activatedObjects.Length * secondsBetweenActivations - secondsBetweenActivations;
    }

    protected override void Update()
    {
        if (timerOn)
        {
            // Wait until time to clean up
            if (!cleaningUp && timeElapsed >= time - cleanUpTime)
            {
                StartCleaningUp();
            }
            // Wait until cleanup is done
            else if (timeElapsed >= time)
            {
                Reset();
            }
            timeElapsed += Time.deltaTime;
        }
    }

    protected void StartCleaningUp()
    {
        cleaningUp = true;
        SetObjects(false);
    }

    protected override void SetObjects(bool active)
    {
        StartCoroutine(InstantiateDelayed(active));
    }

    IEnumerator InstantiateDelayed(bool active)
    {
        bool initialState = active;
        foreach (Transform t in activatedObjects)
        {
            if (initialState != active)
                break;

            Instantiate(smokePuffPrefab, t.position, Quaternion.identity, transform.parent);
            t.gameObject.SetActive(active);

            yield return new WaitForSeconds(secondsBetweenActivations);
        }
    }

    // Turn off switch if in activated state when leaving zone
    protected override void OnDisable()
    {
        if (!gameObject.activeInHierarchy && gameObject.activeSelf && !GetComponent<CircleCollider2D>().enabled)
        {
            // Call base function to disable all objects at once
            base.SetObjects(false);
            Reset();
        }
    }

    protected override void Reset()
    {
        timerOn = false;
        cleaningUp = false;
        SetMusic(false);
        SetSwitch(false);   // Ready switch
    }
}