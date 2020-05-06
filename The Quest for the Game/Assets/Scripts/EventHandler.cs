using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventHandler : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool canPause = true;
    public AudioClip pauseSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        audioSource.PlayOneShot(pauseSound);
    }

    // Loka leik (afþíða fyrst því kallað er úr pásuvalmynd í þetta)
    public void ExitGame()
    {
        Time.timeScale = 1f;
        LoadMainMenu();
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
