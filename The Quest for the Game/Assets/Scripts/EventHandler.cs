using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventHandler : MonoBehaviour
{
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

    void Update()
    {
        // Ef leikmaður ýtir á cancel takka (sjálfgefið esc), fara í aðalvalmynd
        if (Input.GetButton("Cancel"))
        {
            LoadMainMenu();
        }
    }
}
