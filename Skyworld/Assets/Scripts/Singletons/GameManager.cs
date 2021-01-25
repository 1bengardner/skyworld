using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {

    [System.Serializable]
    public struct SpecialItem
    {
        public ItemCollectible item;
        public GameObject itemPrefab;
    }
    public SpecialItem[] specialItems;

    // Return true if game is paused. Set to true to pause the game.
    public bool paused
    {
        get
        {
            return _pauseCount > 0;
        }
        set
        {
            if (value)
            {
                _pauseCount++;
            }
            else if (!value)
            {
                _pauseCount--;
                if (_pauseCount < 0)
                {
                    _pauseCount = 0;
                }
            }
        }
    }
    int _pauseCount = 0;
    public bool toggleKeysEnabled = true;
    public AudioClip progressSound;
    public string fileName;
    public const string defaultName = "Rhupert";

    [HideInInspector]
    public int partsFound = 0;
    [HideInInspector]
    public BackgroundMusic backgroundMusic;
    public const int totalParts = 3;
    [HideInInspector]
    public Zone currentZone;
    [SerializeField]
    Zone respawnZone;
    [SerializeField]
    UIZoneName zoneText;
    ScreenOverlay screenOverlay;
    Animator pauseMenu;

    // Singleton
    public static GameManager Instance
    {
        get
        {
            instance = instance ?? FindObjectOfType<GameManager>();
            return instance;
        }
    }
    private static GameManager instance = null;

    void Awake()
    {
        // Singleton: Destroy GameObject if GameManager exists already
        if (Instance != this)
        {
            Debug.LogWarning("Destroying duplicate GameManager.");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SetFileName(Text name)
    {
        fileName = name.text == "" ? defaultName : name.text;
    }

    void Start()
    {
        screenOverlay = GameObject.FindGameObjectWithTag("ScreenFader").GetComponent<ScreenOverlay>();
        backgroundMusic = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<BackgroundMusic>();
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<Animator>();
        currentZone = FindObjectOfType<Zone>();
        if (currentZone != null)
        {
            Camera.main.backgroundColor = currentZone.color;
            if (respawnZone == null)
            {
                respawnZone = currentZone;
            }
        }
        if (PlayerPrefs.GetInt("Mute") == 1 && !backgroundMusic.muted)
        {
            backgroundMusic.ToggleMute();
        }
    }

    void Update()
    {
        if (toggleKeysEnabled && Input.GetButtonDown("Menu"))
        {
            TogglePauseMenu();
        }
        if (toggleKeysEnabled && Input.GetButtonDown("Mute"))
        {
            backgroundMusic.ToggleMute();
            PlayerPrefs.SetInt("Mute", backgroundMusic.muted ? 1 : 0);
        }
    }

    public void TogglePauseMenu()
    {
        float menuAlpha = pauseMenu.GetComponent<CanvasGroup>().alpha;
        paused = menuAlpha == 0f;
        pauseMenu.SetTrigger(menuAlpha == 0f ? "Fade In" : "Close");
    }

    public void SaveAndQuit()
    {
        StateManager.Instance.Save();
        Application.Quit();
    }

    // Allow the coroutine to run even when caller is deactivated
    public void SwapZonesWrapper(Zone zoneToShow, float transitionSeconds = 1f, Color? fade = null, float pauseSeconds = 0f)
    {
        StartCoroutine(SwapZones(zoneToShow, null, transitionSeconds, fade, pauseSeconds));
    }

    public class BetweenScenes { }

    public IEnumerator Respawn(Transform player)
    {
        // We return control to the caller the moment the scene exits and the player returns to the start
        IEnumerator swapZones = SwapZones(respawnZone, player, 0.75f, null, 0.5f);
        StartCoroutine(swapZones);
        yield return new WaitUntil(() => swapZones.Current is BetweenScenes);
        Camera.main.GetComponent<ShakeEffect>().on = false;
    }

    // Returns BetweenScenes after the current zone is made inactive and zoneToShow is active
    IEnumerator SwapZones(Zone zoneToShow, Transform playerToCenter = null, float transitionSeconds = 1f, Color? fade = null, float pauseSeconds = 0f)
    {
        fade = fade ?? Color.black;

        // Add zone to seen zones
        PlayerPrefs.SetInt(Zone.PrefsKey, Math.Max(PlayerPrefs.GetInt(Zone.PrefsKey, -1), zoneToShow.rank));

        paused = true;
        if (TimerManager.Instance.on)
        {
            TimerManager.Instance.StopTimer();
        }
        if (zoneToShow.music != null && zoneToShow.music != backgroundMusic.audioSource.clip)
        {
            StartCoroutine(backgroundMusic.FadeOutAndIn(zoneToShow.music, transitionSeconds + pauseSeconds));
        }
        yield return StartCoroutine(screenOverlay.Fade(fade.GetValueOrDefault(), transitionSeconds / 2f));
        // Pause on old scene before changing scenes
        if (pauseSeconds > 0f)
            yield return new WaitForSeconds(pauseSeconds);
        if (currentZone != zoneToShow)
        {
            zoneToShow.gameObject.SetActive(true);
            currentZone.gameObject.SetActive(false);
        }
        // Return BetweenScenes when the new scene is active
        yield return new BetweenScenes();
        if (playerToCenter != null)
        {
            playerToCenter.position = zoneToShow.spawnPoint.position + 0.5f * Vector3.up;
            Camera.main.GetComponent<Camera2DFollow>().SnapTo(playerToCenter.transform.position);
        }
        // Show Zone Text
        if (zoneToShow.name != currentZone.name)
        {
            zoneText.infoString = StringTagConverter.ConvertString(zoneToShow.name);
        }
        currentZone = zoneToShow;
        Camera.main.backgroundColor = zoneToShow.color;
        yield return StartCoroutine(screenOverlay.Fade(Color.clear, transitionSeconds / 2f));
        paused = false;
    }
}
