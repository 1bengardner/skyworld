using UnityEngine;
using UnityEngine.UI;

public class UIMuteIcon : MonoBehaviour
{
    public BackgroundMusic backgroundMusic;

    void Awake()
    {
        backgroundMusic.OnToggleMute += ToggleMuteIcon;
    }

    void ToggleMuteIcon()
    {
        if (backgroundMusic.muted != GetComponent<Toggle>().isOn)
        {
            Toggle.ToggleEvent temp = GetComponent<Toggle>().onValueChanged;
            GetComponent<Toggle>().onValueChanged = new Toggle.ToggleEvent();
            GetComponent<Toggle>().isOn = !GetComponent<Toggle>().isOn;
            GetComponent<Toggle>().onValueChanged = temp;
        }
    }
}
