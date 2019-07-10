using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitch : Switch
{
    bool toggled = false;

    protected override IEnumerator Activate(AudioSource clipSource)
    {
        toggled = !toggled;
        clipSource.PlayOneShot(soundClip);
        SetSwitch(false);
        SetObjects(toggled);
        if (activationStopsTimer)
        {
            SetMusic(false, true);
            TimerManager.Instance.StopTimer();
        }
        yield return null;
    }
}
