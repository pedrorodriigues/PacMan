using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

//controlla a pausa inicial do jogo
public class GameStart : MonoBehaviour
{
    int time = 3;
    public static bool Pause = true;
    public CharacterController controller;
    public TextMeshProUGUI CountText;



    void Start()
    {
        
        controller.enabled = false;
        Pause = true;
        StartCoroutine(StartCountdown());
        CountText.text = time.ToString();
    }

    IEnumerator StartCountdown()
    {
        
        while (time >= 0)
        {
            
            yield return new WaitForSeconds(1f);
            CountText.text= time.ToString();
            time -= 1;
        }
        CountText.enabled = false;
        controller.enabled = true;     
        Pause = false;
    }
    

}
