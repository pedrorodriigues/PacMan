using UnityEngine.AI;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;



public class Player : MonoBehaviour
{
    public TextMeshProUGUI pontuação;
    public static float score = 0;
    public GameObject controlGhost;
    enemyIA enemy;
    private Stages Diff;
    public GameObject stg;
    private int totalPoint=0;




    private void Start()
    {
        //pega o script enemyIA de outro objeto
        enemy = controlGhost.GetComponent<enemyIA>();
        //pega o script stages de outro objeto
        Diff = stg.GetComponent<Stages>();
        
    }

    //Caso haja alguma colisão com o jogador
    void OnTriggerEnter(Collider col)
    {
        //colide com as esferas amarelas, incrementa pontuação e verifica se stage foi concluido.
        if (col.gameObject.tag == "PacPoint")
        {
            Destroy(col.gameObject);
            totalPoint += 1;
            score += 1 * Diff.ScoreMult[enemyIA.stage];
            pontuação.text = "Score:" + score.ToString();
            if (totalPoint == 74)
            {
                enemy.NextStage();
            }
        }
        //colide com os fantasmas
        else if (col.gameObject.tag == "Ghosts")
        {
            int i = 0;
            //faz uma comparacao entre o fastama colidido com os fantasmas existestes e descobre qual foi que colidiu
            while ((!GameObject.ReferenceEquals(col.GetComponent<NavMeshAgent>(), enemy.Ghost[i])))
                i++;
            //verfica se o ghost q colideu com o player esta no estado de assustado
            if (enemy.scareds[i])
            {
                score += 100;
                pontuação.text = "Score:" + score.ToString();
                StartCoroutine(enemy.reviveGhost(col.gameObject));
            }
            else
            {

                //chama a tela final depois de colidir com o fantasma sem estar assustado
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  
            }

        }
        //colisão com os potais, move o player para o outro portal.
        else if (col.gameObject.name == "teleportWall1")
        {

            this.GetComponent<CharacterController>().enabled = false;
            this.transform.position = new Vector3(-20.91f, 2.66f, -1.7f);
            this.transform.rotation = new Quaternion(0, 0.9f, 0, 0.4f);
            this.GetComponent<CharacterController>().enabled = true;
            col.GetComponent<AudioSource>().Play();
        }
        else if (col.gameObject.name == "teleportWall2")
        {
            this.GetComponent<CharacterController>().enabled = false;
            this.transform.position = new Vector3(13.35f, 2.66f, -1.89f);
            this.transform.rotation = new Quaternion(0, 0.4f, 0, -0.9f);
            this.GetComponent<CharacterController>().enabled = true;
            col.GetComponent<AudioSource>().Play();
        }
        //colisao com boost coloca os fantasmas em modo assustado
        else if ((col.gameObject.tag == "boost"))
        {
            enemy.ScaredState();
            Destroy(col.gameObject);
        }
        
        

    }
    


}