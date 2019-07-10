using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour {

    // Singleton
    public static TimerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TimerManager>();
            }
            return instance;
        }
    }
    private static TimerManager instance = null;

    [SerializeField]
    Text timeText;
    [SerializeField]
    Image timeImage;
    [SerializeField]
    AudioClip tickClip;
    [SerializeField]
    AudioClip overClip;
    [HideInInspector]
    public bool on;
    float duration;
    float timeElapsed;
    AudioSource audioSource;

    void Awake()
    {
        // Singleton: Destroy GameObject if TimerManager exists already
        if (Instance != this)
        {
            Debug.LogWarning("Destroying duplicate TimerManager.");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

	public void StartTimer (float duration) {
        timeElapsed = 0f;
        this.duration = duration;
        timeText.enabled = true;
        timeImage.enabled = true;
        audioSource.Play();
        on = true;
	}

    public void StopTimer()
    {
        on = false;
        audioSource.Stop();
        audioSource.PlayOneShot(overClip);
        timeElapsed = 0f;
        timeText.enabled = false;
        timeImage.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		if (on)
        {
            if (timeElapsed < duration)
            {
                timeText.text = ((int)(duration - timeElapsed + 1f)).ToString();
                timeText.color = new Color(timeText.color.r, timeText.color.g, timeText.color.b, (duration - timeElapsed) % 1);
                timeImage.fillAmount = (duration - timeElapsed) / duration;
            }
            else
            {
                StopTimer();
            }
            timeElapsed += Time.deltaTime;
        }
	}
}
