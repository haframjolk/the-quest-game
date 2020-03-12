using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventHandler : MonoBehaviour
{
    void Update()
    {
        // Ef leikmaður ýtir á cancel takka (sjálfgefið esc), fara í aðalvalmynd
        if (Input.GetButton("Cancel"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
