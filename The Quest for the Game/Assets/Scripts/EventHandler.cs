using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventHandler : MonoBehaviour
{
    // Hlaða inn aðalvalmynd
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Hlaða inn senum
    public void LoadSvennzHouse()
    {
        SceneManager.LoadScene("SvennzHouse");
    }

    public void LoadRaudalaekur()
    {
        SceneManager.LoadScene("Raudalaekur");
    }

    public void LoadNewZealand()
    {
        SceneManager.LoadScene("NewZealand");
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
