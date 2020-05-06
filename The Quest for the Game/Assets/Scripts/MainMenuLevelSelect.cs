using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLevelSelect : MonoBehaviour
{
    public void GoToSvennzHouse()
    {
        SceneManager.LoadScene("SvennzHouse");
    }
    public void GoToRaudalaekur()
    {
        SceneManager.LoadScene("Raudalaekur");
    }
    public void GoToNewZealand()
    {
        SceneManager.LoadScene("NewZealand");
    }
    public void GoToYonasCastle()
    {
        SceneManager.LoadScene("YonasCastle");
    }
}
