using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Text versionText;
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
        // Setja útgáfunúmer leiksins í version textann
        versionText.text = Application.version;
    }
}
