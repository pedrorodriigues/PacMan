using UnityEngine.AI;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;



public class enemyIA : MonoBehaviour
{


    public Transform Player;
    public NavMeshAgent[] Ghost = new NavMeshAgent[4];
    Vector3[] CornerPositon = new[] { new Vector3(14, 0, -22), new Vector3(-21, 0, -21), new Vector3(-22, 0, 20), new Vector3(15, 0, 20) };
    Vector3[] Position1 = new[] { new Vector3(11.3f, 0, -10.87f), new Vector3(-5.45f, 0, -18.28f), new Vector3(-5.75f, 0, 18.66f) ,new Vector3(-1.38f, 0, 18.47f) };
    Vector3[] Position2 = new[] { new Vector3(7.51f, 0, -19.48f), new Vector3(-17.23f, 0, -14.77f), new Vector3(-14.64f, 0, 14.96f), new Vector3(7.99f, 0, 12.67f) };
    Stages Diff = new Stages();
    float distance, refindWhen, now, refind,cornerTime;
    public static int stage;
    int sort;
    private bool[] backCorner = new bool[] { false, false, false, false, false };
    public bool[] scareds = new bool[] { false, false, false, false, false };
    public bool[] isStarts = new bool[4] { true, true, true, true };
    Color[] original =new Color[4];
    public bool scared = true;
    public GameObject nextMenuUI;
    private Coroutine scaredRoutine;
        



    public void G_Find(int i)
    {
       
        if (i == 0)
        {        
            Ghost[i].destination = Player.position;
        }
        else if (i == 1)
        {
            if (Vector3.Distance(Ghost[i].transform.position, Player.position) <= 5.5)
                Ghost[1].destination = Player.position ;
            else
                Ghost[1].destination = Player.position + Player.forward * 5.5f;
        }
        else if (i == 2)
        {
            distance = Vector3.Distance(Ghost[i].transform.position, Player.position);
            if (distance < 8)
            {
                Ghost[i].destination = CornerPositon[i];
            }
            else
            {
                Ghost[i].destination = Player.position;
            }
        }
        else
        {
            if (Vector3.Distance(Ghost[i].transform.position, Player.position) <= 5.5)
                Ghost[1].destination = Player.position;
            else
                Ghost[i].destination = Player.position + Player.forward * -5.5f;
        }
    }

    public void ScaredState()
    {
        MeshRenderer temp;
        if (scaredRoutine == null)
        {
            for (int i = 0; i < 4; i++)
            {
                temp = Ghost[i].GetComponentInChildren<MeshRenderer>();
                original[i] = temp.material.color;
                temp.material.SetColor("_Color", Color.blue);


                Ghost[i].speed = 4f;
                Ghost[i].destination = CornerPositon[i];
                scareds[i] = true;
                StartCoroutine(FixCornerLock(i));
                
            }
            Debug.Log(scaredRoutine);
            scaredRoutine = StartCoroutine(WaiScaredEnd());
            Debug.Log(scaredRoutine);


        }
        else
        {
            StopCoroutine(scaredRoutine);
            scaredRoutine = StartCoroutine(WaiScaredEnd());
        }
     
    }
    public IEnumerator WaiScaredEnd()
    {
        yield return new WaitForSeconds(15f);
        for (int i = 0; i < 4; i++)
            EndScareState(i);
        scaredRoutine = null;
        
    }


    public void NextDiff()
    {
        nextMenuUI.SetActive(true);
        Time.timeScale = 0;
           


    }
    public void NextStage()
    {
        stage += 1;
        nextMenuUI.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Debug.Log(Pontuacao.score);
        //aumentado score so pra testar e n bugar em loop
        //Pontuacao.score += 1;
        //Debug.Log(stage);
        Prep_Stage();
    }
    IEnumerator StartG(int i)
    {
        //Debug.Log("startg1 " + i);
        yield return new WaitForSeconds(Diff.startTime[stage][i]);
        //Debug.Log("startg2" +i);
        //Debug.Log("startg3" + i);
        StartCoroutine(Corner(i));
        StartCoroutine(Refind(i));
       
    }

    public IEnumerator FixCornerLock(int i)
    {
        int step = 0;
       
        while (backCorner[i] || scareds[i])
        {
            //Debug.Log(i);
            if (Vector3.Distance(Ghost[i].transform.position, CornerPositon[i]) < 0.5 && step==0)
            {
                Ghost[i].destination = Position1[i];
                step++;
            }else if(Vector3.Distance(Ghost[i].transform.position, Position1[i]) < 0.5 && step==1)
            {
                Ghost[i].destination = Position2[i];
                step++;
            }
            else if (Vector3.Distance(Ghost[i].transform.position, Position2[i]) < 0.5 && step == 2)
            {
                Ghost[i].destination = CornerPositon[i];
                step = 0;
            }
            yield return null;
        }
    }
        
    IEnumerator Corner(int i)
    {
        while (true)
        {
            if (!scareds[i])
            {
                if (!isStarts[i])
                    yield return new WaitForSeconds(Diff.cornerTime[stage][i]);
                Ghost[i].destination = CornerPositon[i];
                backCorner[i] = true;
                StartCoroutine(FixCornerLock(i));
                isStarts[i] = false;
                yield return new WaitForSeconds(10f);
                backCorner[i] = false;
            }
            yield return null;

        }
            
    }
    IEnumerator rndCorner()
    {
        while (true)
        {
            yield return new WaitForSeconds(12f);
                sort = Random.Range(0, 4);
        }
    }

    IEnumerator Refind(int i)
    {
        while (true)
        {
            yield return new WaitForSeconds(refindWhen);
           
            if (!backCorner[i]  && !scareds[i] && !isStarts[i])
            {
                //Debug.Log("refind " + i);
                if (sort != i)
                {
                    G_Find(i);
                }
                else
                {
                    Ghost[i].destination = CornerPositon[i];
                    sort = 5;
                    yield return new WaitForSeconds(4f);
                }
            }
               
                   
               
        }

    }
    public IEnumerator reviveGhost(GameObject ghost)
    {
        int i = 0;
        ghost.SetActive(false);        
        ghost.transform.position = new Vector3(-5.28f, 0.8332602f, -2.31f);
        yield return new WaitForSeconds(5f);
        ghost.SetActive(true);
        while ((!GameObject.ReferenceEquals(ghost.GetComponent<NavMeshAgent>(), Ghost[i])))
            i++;
        EndScareState(i);



    }

    public void EndScareState(int i)
    {    
        MeshRenderer temp = Ghost[i].GetComponentInChildren<MeshRenderer>();
        temp.material.color = original[i];
        scareds[i] = false;
        Ghost[i].speed = 5f;
    }

    public void Prep_Stage()
    {
        refindWhen = Diff.refind[stage][0];         
        for (int i=0;i <4; i++)
        {
            StartCoroutine(StartG(i));
                
        }
        StartCoroutine(rndCorner());


    }
    private void Start()
    {
       
        Prep_Stage();
    }

       


    void Update()
    {
   
        if (Pontuacao.score == 195)
        {
            NextDiff();
        }
        
    }
}
