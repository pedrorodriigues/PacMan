using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSound : MonoBehaviour
{

    public AudioSource menuSelectSound;
    public AudioSource menuMainSong;
    public GameObject mute, unmute;

    public void playSelectMenu()
    {
        
        menuSelectSound.Play();
    }

    public void PauseResume()
    {
        if (menuMainSong.isPlaying)
        {
            menuMainSong.Pause();
            mute.SetActive(true);
            unmute.SetActive(false);
        }

        else{
            menuMainSong.Play();
            mute.SetActive(false);
            unmute.SetActive(true);
        }
           
    }
}

