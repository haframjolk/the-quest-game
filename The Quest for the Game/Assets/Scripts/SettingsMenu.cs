﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{   
    public Toggle fullScreenToggle;
    public GameObject settingsButton;
    public GameObject canvas;
    public GameObject resolutionSettings;
    public Dropdown resolutionDropdown;
    public char resolutionDelimiter = '\u00D7';  // Stafurinn milli breiddar og hæðar í upplausnunum í dropdown-valmyndinni
    public List<Resolution> availableResolutions;
    private Resolution selectedResolution;
    private bool fullScreenEnabled;
    private bool toggleEnabled = true;
    private bool resolutionChangeEnabled = true;

    // Sýna og fela settings menu
    public void SetActive(bool value)
    {
        canvas.SetActive(value);
        settingsButton.SetActive(!value);  // Fela settings takka þegar kveikt er á menu
    }

    // Stilla (windowed) upplausn
    public void SetResolution(Resolution resolution)
    {
        Screen.SetResolution(resolution.width, resolution.height, false);
    }

    // Stilla (windowed) upplausn með indexi (frá dropdown menu)
    public void SetResolution(int index)
    {
        if (!resolutionChangeEnabled)
        {
            return;
        }
        
        selectedResolution = availableResolutions[index];
        SetResolution(selectedResolution);
        // Debug.Log($"Windowed resolution set to {selectedResolution.width}*{selectedResolution.height}.");
    }

    // Stilla upplausn á native upplausn, full screen
    void EnableFullScreen()
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
    }

    // Stilla toggle eftir því hvort kveikt er á full screen eða ekki
    void UpdateToggle()
    {
        // Slökkva á toggleEnabled á meðan svo leikurinn reyni ekki að skipta úr full screen í windowed þegar viðmótið er uppfært
        toggleEnabled = false;
        fullScreenToggle.isOn = fullScreenEnabled;
        toggleEnabled = true;
    }

    void Start()
    {
        fullScreenEnabled = Screen.fullScreen;

        // Ef leikurinn er í full screen en upplausnin er ekki native, laga það
        if (fullScreenEnabled && (Screen.width != Screen.currentResolution.width || Screen.height != Screen.currentResolution.height))
        {
            EnableFullScreen();
        }

        UpdateToggle();

        // Halda utan um upplausnir sem eru í boði í stillingunum á dropdowninu
        availableResolutions = new List<Resolution>();
        foreach (Dropdown.OptionData resolution in resolutionDropdown.options)
        {
            string[] resolutionParts = resolution.text.Split(resolutionDelimiter);

            Resolution newResolution = new Resolution();
            newResolution.width = System.Int32.Parse(resolutionParts[0]);
            newResolution.height = System.Int32.Parse(resolutionParts[1]);

            availableResolutions.Add(newResolution);
        }

        // Ef ekki er kveikt á full screen
        int resolutionIndex = 0;
        if (!fullScreenEnabled)
        {
            // Athuga hvort núverandi upplausn sé til í listanum yfir upplausnir
            resolutionIndex = availableResolutions.FindIndex(res => res.width == Screen.width && res.height == Screen.height);

            // Ef rétta upplausnin fannst ekki
            if (resolutionIndex == -1)
            {
                // Velja fyrstu upplausnina sem sjálfgefna
                resolutionIndex = 0;
            }

        }
        // Stilla rétta upplausn í dropdown menu-inu (passa að skjástillingar breytist ekki með því að stilla resolutionChangeEnabled = false)
        resolutionChangeEnabled = false;
        resolutionDropdown.value = resolutionIndex;
        resolutionChangeEnabled = true;

        // Halda utan um valda upplausn
        selectedResolution = availableResolutions[resolutionIndex];

        // Bara sýna upplausnarstillingu ef slökkt er á full screen
        resolutionSettings.SetActive(!fullScreenEnabled);
    }

    // Kveikja/slökkva á full screen
    public void ToggleFullScreen()
    {
        if (!toggleEnabled)
        {
            return;
        }

        fullScreenEnabled = !fullScreenEnabled;

        if (fullScreenEnabled)
        {
            EnableFullScreen();
        }
        else
        {
            SetResolution(selectedResolution);
        }

        UpdateToggle();
        
        resolutionSettings.SetActive(!fullScreenEnabled);  // Sýna upplausnarstillingar bara ef slökkt er á full screen
    }
}
