using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZSwitch : Switch
{
    static event Action OnIncorrect;
    [SerializeField]
    ZSwitch previous;
    [SerializeField]
    AudioClip wrongClip;
    event Action OnActivated;
    bool ready;

    private void Start()
    {
        OnIncorrect += delegate
        {
            if (!GetComponent<CircleCollider2D>().enabled)
            {
                SetSwitch(false);
                Instantiate(smokePuffPrefab, transform.position, Quaternion.identity, transform.parent);
            }
            if (previous)
            {
                ready = false;
            }
        };

        if (!previous)
        {
            ready = true;
        }
        else
        {
            previous.OnActivated += delegate { ready = true; };
        }
    }

    protected override IEnumerator Activate(AudioSource clipSource)
    {
        if (GetComponent<CircleCollider2D>().enabled)
        {
            if (ready)
            {
                yield return base.Activate(clipSource);
                OnActivated();
            }
            else
            {
                clipSource.PlayOneShot(soundClip);
                SetSwitch(true);
                yield return new WaitForSeconds(0.4f);
                clipSource.PlayOneShot(wrongClip);
                OnIncorrect();
            }
        }
    }
}
