using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSound : MonoBehaviour
{

    public AudioSource menuSelectSound;

    public void playSelectMenu()
    {
        menuSelectSound.Play();
    }
}

