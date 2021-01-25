using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMusic : MonoBehaviour {

    public AudioClip clipHurry;
    public AudioClip clipCelebrate;
    public delegate void MuteAction();
    public event MuteAction OnToggleMute;
    [HideInInspector]
    public AudioSource audioSource;
    // Real volume
    public float volume
    {
        // Change the default volume
        set
        {
            defaultVolume = value;
            ChangeVolume(defaultVolume);
        }
        get
        {
            return audioSource.volume;
        }
    }
    public bool muted
    {
        get
        {
            return _muted;
        }
    }
    bool _muted = false;
    struct PausedClip
    {
        public PausedClip(AudioClip clip, float secondsIntoClip)
        {
            this.clip = clip;
            this.secondsIntoClip = secondsIntoClip;
        }
        public AudioClip clip;
        public float secondsIntoClip;
    }
    PausedClip lastSwappedClip; // for SwapClip
    float defaultVolume;
    // Volume that audio would be playing at if it were unmuted
    float _volume;
    bool changingZones;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        defaultVolume = audioSource.volume;
        _volume = defaultVolume;
    }

    void Start() {
        Zone zone = FindObjectOfType<Zone>();
        if (zone == null)
        {
            Debug.LogWarning("BackgroundMusic: No zone enabled to play music from.");
        }
        else
        {
            audioSource.clip = FindObjectOfType<Zone>().music;
            audioSource.Play();
        }
    }

    // Momentarily change the volume from default
    void ChangeVolume(float volume)
    {
        audioSource.volume = _muted ? 0f : volume;
        _volume = volume;
    }

    IEnumerator FadeTo(float toVolume, float seconds)
    {
        float fromVolume = volume;
        float elapsed = 0f;
        while (elapsed <= seconds)
        {
            elapsed += Time.deltaTime;
            ChangeVolume(Mathf.Lerp(fromVolume, toVolume, elapsed / seconds));
            yield return null;
        }
    }

    // Over time, fade the audio out and back to default, optionally changing clips.
    public IEnumerator FadeOutAndIn(AudioClip newClip = null, float seconds = 2f)
    {
        changingZones = true;
        yield return StartCoroutine(FadeTo(0f, seconds / 2f));
        if (newClip != null)
        {
            SwapClip(newClip, true);
        }
        yield return StartCoroutine(FadeTo(defaultVolume, seconds / 2f));
        changingZones = false;
    }

    /*
    Swap the currently playing clip for another and begin playing immediately.
    Play last swapped clip if none specified.
    */
    public void SwapClip(AudioClip newClip = null, bool force = false)
    {
        // Prevent switches from triggering a clip swap after being disabled during zone swap
        if ((changingZones && !force) || newClip == audioSource.clip)
            return;
        float newTime = 0f;
        if (newClip == null)
        {
            newClip = lastSwappedClip.clip;
            newTime = lastSwappedClip.secondsIntoClip;
        }
        lastSwappedClip = new PausedClip(audioSource.clip, audioSource.time);
        audioSource.clip = newClip;
        audioSource.time = newTime;
        audioSource.Play();
    }
    
    public void ToggleMute()
    {
        _muted = !_muted;
        ChangeVolume(_volume);
        if (OnToggleMute != null)
        {
            OnToggleMute();
        }
    }

    public void QuietDown(bool yes)
    {
        float quietVolume = defaultVolume / 2f;
        if (yes)
        {
            StartCoroutine(FadeTo(quietVolume, 0.25f));
        }
        else
        {
            StartCoroutine(FadeTo(defaultVolume, 0.5f));
        }
    }
}
