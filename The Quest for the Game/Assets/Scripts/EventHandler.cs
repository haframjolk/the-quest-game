using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventHandler : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool canPause = true;
    public AudioClip pauseSound;
    public bool cursorEnabled = false;  // Á bendillinn að vera sýnilegur á meðan á leik stendur?
    private AudioSource audioSource;

    // Stilla hvort hægt sé að pása núverandi senu
    public void SetCanPause(bool value)
    {
        canPause = value;
    }

    // Frelsa bendil og sýna hann
    void EnableCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Fela bendil og læsa hann í miðju skjásins
    void DisableCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Ef verið er að keyra leikinn í editornum, sýna bendilinn alltaf (þægilegra að hann hverfi ekki)
        if (Application.isEditor)
        {
            cursorEnabled = true;
        }
        if (!cursorEnabled)
        {
            DisableCursor();
        }
    }

    // Hlaða inn senu
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Hlaða inn aðalvalmynd
    public void LoadMainMenu()
    {
        LoadScene("MainMenu");
    }

    // Hlaða inn senum
    public void LoadSvennzHouse()
    {
        LoadScene("SvennzHouse");
    }

    public void LoadRaudalaekur()
    {
        LoadScene("Raudalaekur");
    }

    public void LoadNewZealand()
    {
        LoadScene("NewZealand");
    }

    public void LoadYonasCastle()
    {
        LoadScene("YonasCastle");
    }

    public void LoadCredits()
    {
        LoadScene("Credits");
    }

    // Pása leik (frysta og sýna menu)
    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        audioSource.PlayOneShot(pauseSound);
        // Sýna bendil í pásuvalmynd
        EnableCursor();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        audioSource.PlayOneShot(pauseSound);
        // Fela bendil aftur ef hann á að vera falinn þegar farið er út úr pásuvalmyndinni
        if (!cursorEnabled)
        {
            DisableCursor();
        }
    }

    // Loka leik (afþíða fyrst því kallað er úr pásuvalmynd í þetta)
    public void ExitGame()
    {
        Time.timeScale = 1f;
        LoadMainMenu();
    }

    // Hætta í leik
    public void QuitGame()
    {
        Application.Quit();
    }

    void Update()
    {
        // Ef leikmaður ýtir á cancel takka (sjálfgefið esc) og má pása þá senu sem hann er í, pása leik
        if (Input.GetButtonDown("Cancel") && canPause)
        {
            if (Time.timeScale == 1f)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }
}
