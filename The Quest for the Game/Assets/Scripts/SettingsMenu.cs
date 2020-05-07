using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Toggle fullScreenToggle;
    public GameObject settingsButton;
    public GameObject canvas;
    bool fullScreenEnabled;

    // Sýna og fela settings menu
    public void SetActive(bool value)
    {
        canvas.SetActive(value);
        settingsButton.SetActive(!value);  // Fela settings takka þegar kveikt er á menu
    }

    void Start()
    {
        fullScreenEnabled = Screen.fullScreen;
        fullScreenToggle.isOn = fullScreenEnabled;
    }

    // Kveikja/slökkva á full screen
    public void ToggleFullScreen()
    {
        fullScreenEnabled = !Screen.fullScreen;
        if (fullScreenEnabled)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
        }
        else
        {
            Screen.SetResolution(800, 600, false);
        }
    }
}
