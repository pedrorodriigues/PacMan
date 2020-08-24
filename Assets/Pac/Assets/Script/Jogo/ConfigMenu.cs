using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject gameTitle;
    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        gameTitle.SetActive(true);
        gameObject.SetActive(false);
    }

    public void CallConfigMenu()
    {
        mainMenu.SetActive(false);
        gameTitle.SetActive(false);
        gameObject.SetActive(true);
    }
}
