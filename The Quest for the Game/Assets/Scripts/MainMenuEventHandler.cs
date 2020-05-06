using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEventHandler : MonoBehaviour
{
    void Update()
    {
        // Ef leikmaður ýtir á cancel takka (sjálfgefið esc), loka leik
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
    }
}
