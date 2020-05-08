using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject[] disabledOnWebGL;
    void Start()
    {
        // Ef leikurinn er að keyra í WebGL, slökkva á því sem á ekki að vera aktíft
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            foreach (GameObject gobject in disabledOnWebGL)
            {
                gobject.SetActive(false);
            }
        }
    }
}
