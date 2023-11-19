using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMusic : MonoBehaviour {

    public AudioClip clipHurry;
    public AudioClip clipCelebrate;
    public delegate void MuteAction();
    public event MuteAction OnToggleMute;
    public Slider musicVolumeSlider;
    [HideInInspector]
    public AudioSource audioSource;
    float universalVolume;
    float _mainVolume;
    public float mainVolume
    {
        set
        {
            _mainVolume = value;
            UpdateVolume();
            PlayerPrefs.SetFloat("Music volume", value);
        }
        private get
        {
            return _mainVolume;
        }
    }
    float _relativeVolume;
    float relativeVolume
    {
        set
        {
            _relativeVolume = value;
            UpdateVolume();
        }
        get
        {
            return _relativeVolume;
        }
    }
    float _sfxVolume;
    public float sfxVolume
    {
        set
        {
            _sfxVolume = value;
            UpdateVolume();
        }
        private get
        {
            return _sfxVolume;
        }
    }
    bool _muted;
    public bool muted
    {
        private set
        {
            _muted = value;
            UpdateVolume();
        }
        get
        {
            return _muted;
        }
    }
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
    bool changingZones;

    void Awake() {
        var initialVolume = musicVolumeSlider.value;
        audioSource = GetComponent<AudioSource>();
        universalVolume = audioSource.volume;
        _mainVolume = initialVolume;
        universalVolume /= initialVolume;
        relativeVolume = 1f;
        sfxVolume = 1f;
    }

    void Start() {
        Zone zone = FindObjectOfType<Zone>();
        if (zone == null)
        {
            Debug.LogWarning("BackgroundMusic: No zone enabled to play music from.");
        }
        else
        {
            audioSource.clip = zone.music;
            audioSource.time = zone.startMusicAtSeconds;
            audioSource.Play();
        }
    }
    
    void UpdateVolume()
    {
        var volume = universalVolume * mainVolume * relativeVolume / sfxVolume;
        audioSource.volume = muted ? 0f : volume;
    }

    IEnumerator FadeTo(float toVolume, float seconds)
    {
        float fromVolume = relativeVolume;
        float elapsed = 0f;
        while (elapsed <= seconds)
        {
            elapsed += Time.deltaTime;
            relativeVolume = Mathf.Lerp(fromVolume, toVolume, elapsed / seconds);
            yield return null;
        }
    }

    // Over time, fade the audio out and back to default, optionally changing clips.
    public IEnumerator FadeOutAndIn(AudioClip newClip = null, float fadeSeconds = 2f, float startAtSeconds = 0f)
    {
        changingZones = true;
        yield return StartCoroutine(FadeTo(0f, fadeSeconds / 2f));
        if (newClip != null)
        {
            SwapClip(newClip, true, startAtSeconds);
        }
        yield return StartCoroutine(FadeTo(1f, fadeSeconds / 2f));
        changingZones = false;
    }

    /*
    Swap the currently playing clip for another and begin playing immediately.
    Play last swapped clip if none specified.
    */
    public void SwapClip(AudioClip newClip = null, bool force = false, float startTimeInSeconds = 0f)
    {
        // Prevent switches from triggering a clip swap after being disabled during zone swap
        if ((changingZones && !force) || newClip == audioSource.clip)
            return;
        float newTime = startTimeInSeconds;
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
        muted = !muted;
        if (OnToggleMute != null)
        {
            OnToggleMute();
        }
    }

    public void QuietDown(bool yes, float quietVolume = 0.6f)
    {
        if (yes)
        {
            StartCoroutine(FadeTo(quietVolume, 0.25f));
        }
        else
        {
            StartCoroutine(FadeTo(1f, 0.5f));
        }
    }
}
