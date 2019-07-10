using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleCutscene : Cutscene
{
    public Zone startingZone;
    [SerializeField]
    ScreenOverlay titleOverlay;
    [SerializeField]
    ScreenOverlay mainOverlay;
    [SerializeField]
    Transform followTarget;
    [SerializeField]
    PlayerUserControl controls;

    new void Start()
    {
        base.Start();
        controls.enabled = false;
        Camera.main.GetComponent<Camera2DFollow>().target = followTarget;
        GameManager.Instance.toggleKeysEnabled = false;
        titleOverlay.SetColor(Color.black);
        mainOverlay.SetColor(Color.clear);
        // Fade screen from white to clear
        ScreenOverlay fader = GameObject.FindGameObjectWithTag("ScreenFader").GetComponent<ScreenOverlay>();
        fader.SetColor(Color.white);
        StartCoroutine(fader.Fade(new Color(1f, 1f, 1f, 0f), 0.25f));
    }

    IEnumerator StartGame()
    {
        float fadeSeconds = 2f;
        float pauseSeconds = 0.5f;
        GameManager.Instance.SwapZonesWrapper(startingZone, fadeSeconds, Color.white, pauseSeconds);
        yield return StartCoroutine(titleOverlay.Fade(Color.clear, fadeSeconds / 2));
        yield return new WaitForSeconds(pauseSeconds);
        StartCoroutine(mainOverlay.Fade(Color.black, fadeSeconds / 2));
        controls.enabled = true;
    }
}
