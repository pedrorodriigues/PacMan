using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonVR : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider loadBar;



    //funcao resposavel por clicar no botao depois de focar por um tempo, alem de controlar o slider
    IEnumerator WaitOnGazeEnterToStart(int opt)
    {
        loadBar.gameObject.SetActive(true);
        float time=0;
        while (time<2)
        {
            time += Time.deltaTime;
            loadBar.value = time / 2;
            yield return null;
        }
        if (opt == 1)
            StartGame();  
        else
            QuitGame();
    }
   

    public void StartGame()
    {
        Player.score = 0;
        enemyIA.stage = 0;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GazeEnterPlay(int opt)
    {       
        StartCoroutine(WaitOnGazeEnterToStart(opt));
    }

    //sair do foco no botao, mata todas as rotinas e esconde slider
    public void GazeExit()
    {
        StopAllCoroutines();
        loadBar.gameObject.SetActive(false);
        loadBar.value = 0;
    }

   
}
