using UnityEngine.AI;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;



public class Pontuacao : MonoBehaviour
{
    public TextMeshProUGUI pontuação;
    public static float score = 0;
    
    bool scare;
    public GameObject controlGhost;
    enemyIA enemy;
    public GameObject test;
    private Stages Diff;
    public GameObject stg;




    private void Start()
    {
        enemy = controlGhost.GetComponent<enemyIA>();
        scare = enemy.scared;
        Diff = stg.GetComponent<Stages>();
    }

    void OnTriggerEnter(Collider col)
    {


        if (col.gameObject.tag == "PacPoint")
        {
            
            //Debug.Log("audio aqui");
            //test.GetComponent<AudioSource>().Play();
            Destroy(col.gameObject);
            score += 1 * Diff.ScoreMult[enemyIA.stage];
            pontuação.text = "Score:" + score.ToString();
        }
        else if (col.gameObject.tag == "Ghosts")
        {
            int i = 0;
            while ((!GameObject.ReferenceEquals(col.GetComponent<NavMeshAgent>(), enemy.Ghost[i])))
                i++;
            if (enemy.scareds[i])
            {
                score += 100;
                pontuação.text = "Score:" + score.ToString();
                StartCoroutine(enemy.reviveGhost(col.gameObject));
            }
            else
            {

                // firstpersoncontroller a = this.GetComponent < "First Person Controller" >();
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
               // this.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled=false;
            }

        }
        else if (col.gameObject.name == "teleportWall1")
        {

            this.GetComponent<CharacterController>().enabled = false;
            this.transform.position = new Vector3(-20.91f, 2.66f, -1.7f);
            //Debug.Log(col.transform.position);
            this.transform.rotation = new Quaternion(0, 0.9f, 0, 0.4f);
            //Debug.Log(col.transform.position);
            this.GetComponent<CharacterController>().enabled = true;
        }
        else if (col.gameObject.name == "teleportWall2")
        {
            Debug.Log(col.transform.position);
            this.GetComponent<CharacterController>().enabled = false;
            this.transform.position = new Vector3(13.35f, 2.66f, -1.89f);
            //Debug.Log(col.transform.position);
            this.transform.rotation = new Quaternion(0, 0.4f, 0, -0.9f);
            //Debug.Log(col.transform.position);
            this.GetComponent<CharacterController>().enabled = true;
        }
        else if ((col.gameObject.tag == "boost"))
        {
           enemy.ScaredState();
            Destroy(col.gameObject);
        }
        
        

    }
    


}