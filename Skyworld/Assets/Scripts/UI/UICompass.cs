using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICompass : MonoBehaviour {

    [SerializeField]
    Transform player;
    Image timerImage;
    float timerSeconds;
    float secondsElapsed = 0f;
    bool on = false;

    void OnEnable()
    {
        transform.SetAsLastSibling();
    }

	// Use this for initialization
	void Start () {
        PhaseShift compass = player.GetComponentInChildren<PhaseShift>();
        if (compass != null)
        {
            compass.OnPhaseShift += StartCountdown;
        }
        timerSeconds = compass.secondsBetweenShifts;

        timerImage = GetComponent<Image>();
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (on && secondsElapsed > timerSeconds)
        {
            on = false;
            timerImage.fillAmount = 1f;
            GetComponent<Animator>().SetTrigger("Finish");
        }
        else if (on)
        {
            timerImage.fillAmount = secondsElapsed / timerSeconds;
            secondsElapsed += Time.deltaTime;
        }
    }

    void StartCountdown()
    {
        on = true;
        gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("Start");
        timerImage.fillAmount = 0f;
        secondsElapsed = 0f;
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
