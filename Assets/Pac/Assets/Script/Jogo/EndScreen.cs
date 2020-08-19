using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI pontuacao;
    public TextMeshProUGUI stage;
    void Start()
    {
        
        WriteScoreStage();
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        Player.score = 0;
        enemyIA.stage = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void WriteScoreStage()
    {
        pontuacao.text = "Sua pontuação maxima foi: " + Player.score;
        stage.text = "Voce Atingiu a dificuldade: " + (enemyIA.stage+1);


    }
}
