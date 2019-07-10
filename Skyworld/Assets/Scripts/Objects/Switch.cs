using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, ISavable
{
    protected static int activatedMusicSwitches;

    [SerializeField]
    protected AudioClip soundClip;
    [SerializeField]
    protected Transform[] activatedObjects;
    [SerializeField]
    protected GameObject smokePuffPrefab;
    [SerializeField]
    protected bool activationStopsTimer;
    [SerializeField]
    protected bool zoomOnActivate = false;
    bool zooming = false;

    protected virtual void SetSwitch(bool pressed)
    {
        GetComponent<CircleCollider2D>().enabled = !pressed;
        GetComponent<SpriteSwap>().Swap();  // Warning: This will swap the sprite regardless of pressed
    }

    protected virtual void SetObjects(bool active)
    {
        foreach (Transform t in activatedObjects)
        {
            t.gameObject.SetActive(active);
            Instantiate(smokePuffPrefab, t.position, Quaternion.identity, t.parent);
        }
    }
    
    protected virtual void SetMusic(bool on, bool force = false)
    {
        activatedMusicSwitches += on ? 1 : -1;
        if (force)
            activatedMusicSwitches = on ? 1 : 0;
        // Only change music if this is the first or last music switch activated
        if (on && activatedMusicSwitches == 1 || !on && activatedMusicSwitches == 0)
            GameManager.Instance.backgroundMusic.SwapClip(on ? GameManager.Instance.backgroundMusic.clipHurry : null);
        if (activatedMusicSwitches < 0)
            activatedMusicSwitches = 0;
    }

    protected virtual IEnumerator Activate(AudioSource clipSource)
    {
        clipSource.PlayOneShot(soundClip);
        SetSwitch(true);
        if (zoomOnActivate && activatedObjects.Length > 0)
        {
            zooming = true;
            Camera.main.GetComponent<Camera2DFollow>().SetTempTargetWrapper(activatedObjects[0], 1.5f);
            yield return new WaitForSeconds(0.5f);
            zooming = false;
        }
        SetObjects(true);
        if (activationStopsTimer && TimerManager.Instance.on)
        {
            SetMusic(false, true);
            TimerManager.Instance.StopTimer();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(Activate(other.GetComponent<AudioSource>()));
        }
    }

    protected virtual void OnDisable()
    {
        if (zooming)
        {
            SetObjects(true);
        }
    }

    StateRecord ISavable.GetRecord()
    {
        SwitchRecord record = (SwitchRecord)RecordFactory.Get(this);
        record.pressed = gameObject.GetComponent<CircleCollider2D>().enabled;
        return record;
    }

    void ISavable.SetData(StateRecord loaded)
    {
        SwitchRecord record = (SwitchRecord)loaded;
        if (record.pressed == gameObject.GetComponent<CircleCollider2D>().enabled)
        {
            SetSwitch(record.pressed);
            SetObjects(record.pressed);
        }
    }
}
