using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI pontuacao;
    public TextMeshProUGUI stage;
    // Start is called before the first frame update
    void Start()
    {
        
        WriteScoreStage();
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("xd");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
    public void WriteScoreStage()
    {
        pontuacao.text = "Sua pontuação maxima foi: " + Pontuacao.score;
        stage.text = "Voce Atingiu a dificuldade: " + enemyIA.stage;

    }
}
