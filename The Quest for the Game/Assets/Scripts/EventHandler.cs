using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventHandler : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool canPause = true;
    public AudioClip pauseSound;
    public bool cursorAlwaysEnabled = false;  // Á bendillinn alltaf að vera sýnilegur á meðan á leik stendur?
    private bool cursorEnabled = false;  // Á bendillinn að vera sýnilegur þessa stundina?
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
        // Ef verið er að keyra leikinn í WebGL eða í editornum, sýna bendilinn alltaf
        // Það er þægilegra að hann hverfi ekki í editornum og vafrar eru með meldingar og leiðir til að losa bendil með esc takka, sem er óþarfi að díla við
        if (Application.platform == RuntimePlatform.WebGLPlayer || Application.isEditor)
        {
            cursorAlwaysEnabled = true;
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
        cursorEnabled = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        audioSource.PlayOneShot(pauseSound);
        cursorEnabled = false;
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
        // Ef bendillinn er sýnilegur, ef hann á alltaf að vera sýnilegur eða ef leikurinn er ekki í „full screen“, sýna bendil
        if (cursorEnabled || cursorAlwaysEnabled || !Screen.fullScreen)
        {
            EnableCursor();
        }
        // Annars fela hann
        else
        {
            DisableCursor();
        }
    }
}
