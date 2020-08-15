using UnityEngine.AI;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;



public class enemyIA : MonoBehaviour
{
    public Transform PlayerTarget;
    public NavMeshAgent[] Ghost = new NavMeshAgent[4];
    //define as posições de canto para cada fantasma. 
    Vector3[] CornerPositon = new[] { new Vector3(14, 0, -22), new Vector3(-21, 0, -21), new Vector3(-22, 0, 20), new Vector3(15, 0, 20) };
    Vector3[] Position1 = new[] { new Vector3(11.3f, 0, -10.87f), new Vector3(-5.45f, 0, -18.28f), new Vector3(-5.75f, 0, 18.66f) ,new Vector3(-1.38f, 0, 18.47f) };
    Vector3[] Position2 = new[] { new Vector3(7.51f, 0, -19.48f), new Vector3(-17.23f, 0, -14.77f), new Vector3(-14.64f, 0, 14.96f), new Vector3(7.99f, 0, 12.67f) };

    private Stages Diff;
    public GameObject stg, nextMenuUI;
    float distance, refindWhen;
    public static int stage;
    int sort;
    private bool[] backCorner = new bool[] { false, false, false, false, false };
    public bool[] scareds = new bool[] { false, false, false, false, false };
    public bool[] isStarts = new bool[4] { true, true, true, true };
    Color[] original =new Color[4];
    private Coroutine scaredRoutine;
    


    //arrumado
    public void G_Find(int i)
    {
       
        if (i == 0)
        {        
            Ghost[i].destination = PlayerTarget.position;
        }
        else if (i == 1)
        {
            if (Vector3.Distance(Ghost[i].transform.position, PlayerTarget.position) <= Diff.forward[stage][i])
                Ghost[i].destination = PlayerTarget.position ;
            else
                Ghost[i].destination = PlayerTarget.position + PlayerTarget.forward * Diff.forward[stage][i];
        }
        else if (i == 2)
        {
            distance = Vector3.Distance(Ghost[i].transform.position, PlayerTarget.position);
            if (distance < Diff.forward[stage][i])
            {
                Ghost[i].destination = CornerPositon[i];
            }
            else
            {
                Ghost[i].destination = PlayerTarget.position;
            }
        }
        else
        {
            if (Vector3.Distance(Ghost[i].transform.position, PlayerTarget.position) <= Diff.forward[stage][i])
                Ghost[1].destination = PlayerTarget.position;
            else
                Ghost[i].destination = PlayerTarget.position + PlayerTarget.forward * -Diff.forward[stage][i];
        }
    }

    //arrumado
    IEnumerator ChangeColorInFinalScaredState()
    {
        float timeToWait = Diff.ScaredStateTime[stage]/2;
        bool white = false;
        bool keepRun = true;
        float timeTotal = 0; ;
        while (keepRun)
        {
            yield return new WaitForSeconds(timeToWait);
            timeTotal += timeToWait;
            for (int i = 0; i < 4; i++)
            {
                if (scareds[i] && !white)
                {
                    Ghost[i].GetComponentInChildren<MeshRenderer>().material.color = Color.white;
                }
                else if (scareds[i] && white)
                {
                    Ghost[i].GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
                }

            }
            if (white)
                white = false;
            else
                white = true;
            if (timeTotal >= Diff.ScaredStateTime[stage]-3)
                timeToWait = 0.1f;
            else
                timeToWait = 0.5f;


            keepRun = scareds.Any(x=>x==true);
            Debug.Log(keepRun);
        }
        
        
   
    }
   
    //arrumado
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


                Ghost[i].speed = Diff.speed[stage][i]/2;
                Ghost[i].destination = CornerPositon[i];
                scareds[i] = true;
                StartCoroutine(FixCornerLock(i));
                
            }
            StartCoroutine(ChangeColorInFinalScaredState());
            scaredRoutine = StartCoroutine(WaiScaredEnd());
          


        }
        else
        {
            StopCoroutine(scaredRoutine);
            scaredRoutine = StartCoroutine(WaiScaredEnd());
        }
     
    }
    
    //arrumado
    public IEnumerator WaiScaredEnd()
    {
        yield return new WaitForSeconds(Diff.ScaredStateTime[stage]);
        for (int i = 0; i < 4; i++)
            EndScareState(i);
        scaredRoutine = null;
        
    }

    //arrumado****
    public void NextDiff()
    {
        nextMenuUI.SetActive(true);
        Time.timeScale = 0;
    }

    //arrumado****
    public void NextStage()
    {
        stage += 1;
        nextMenuUI.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Prep_Stage();
    }

    //arrumado
    IEnumerator StartG(int i)
    {
        yield return new WaitForSeconds(Diff.startTime[stage][i]);
        StartCoroutine(Corner(i));
        StartCoroutine(Refind(i));   
    }

    //arumado
    public IEnumerator FixCornerLock(int i)
    {
        int step = 0;
       
        while (backCorner[i] || scareds[i])
        {
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
    
    //arrumado
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
                yield return new WaitForSeconds(Diff.RefindAfterCorner[stage][i]);
                backCorner[i] = false;
            }
            yield return null;

        }
            
    }

    //arrumado
    IEnumerator rndCorner()
    {
        while (true)
        {
            yield return new WaitForSeconds(Diff.rndCornerTime[stage]);
                sort = Random.Range(0, 4);
        }
    }

    //arrumado
    IEnumerator Refind(int i)
    {
        while (true)
        {
            yield return new WaitForSeconds(Diff.refind[stage]);
           
            if (!backCorner[i]  && !scareds[i] && !isStarts[i])
            {
                if (sort != i)
                {
                    G_Find(i);
                }
                else
                {
                    Ghost[i].destination = CornerPositon[i];
                    yield return new WaitForSeconds(5f);
                }
            }
               
                   
               
        }

    }

    //arrumado
    public IEnumerator reviveGhost(GameObject ghost)
    {
        int i = 0;
        ghost.SetActive(false);        
        ghost.transform.position = new Vector3(-5.28f, 0.8332602f, -2.31f);
        yield return new WaitForSeconds(Diff.timeToRespawn[stage]);
        ghost.SetActive(true);
        while ((!GameObject.ReferenceEquals(ghost.GetComponent<NavMeshAgent>(), Ghost[i])))
            i++;
        EndScareState(i);



    }

    //arrumado
    public void EndScareState(int i)
    {    
        MeshRenderer temp = Ghost[i].GetComponentInChildren<MeshRenderer>();
        temp.material.color = original[i];
        scareds[i] = false;
        Ghost[i].speed = Diff.speed[stage][i];
    }

    //arrumado
    public void Prep_Stage()
    {
                 
        for (int i=0;i <4; i++)
        {
            StartCoroutine(StartG(i));
                
        }
        StartCoroutine(rndCorner());


    }

    //arrumado
    private void Start()
    {
        Diff = stg.GetComponent<Stages>();
        Prep_Stage();
    }


    void Update()
    {
   
        if (Player.score == 195)
        {
            NextDiff();
        }
        
    }
}
