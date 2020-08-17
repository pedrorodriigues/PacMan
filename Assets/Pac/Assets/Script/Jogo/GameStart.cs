using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GameStart : MonoBehaviour
{
    int time = 5;
    public static bool Pause = true;
    public CharacterController controller;
   

    void Start()
    {

        controller.enabled = false;
        
        Pause = true;
        StartCoroutine(StartCountdown());   
    }

    IEnumerator StartCountdown()
    {
        TextMeshProUGUI CountText = this.gameObject.GetComponent<TextMeshProUGUI>();
        while (time >= 0)
        {
            yield return new WaitForSeconds(1f);
            CountText.text= "" + time;
            time -= 1;
           

        }
        CountText.enabled = false;
        controller.enabled = true;
        
        Pause = false;


    }
    

}
