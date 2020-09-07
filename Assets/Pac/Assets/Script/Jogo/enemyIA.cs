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
    public static int stage=0;
    int sort=10;
    private bool[] backCorner = new bool[] { false, false, false, false, false };
    public bool[] scareds = new bool[] { false, false, false, false, false };
    public bool[] isStarts = new bool[4] { true, true, true, true };
    Color[] original =new Color[4];
    private Coroutine scaredRoutine;
    private Coroutine ColorRoutine;
    private int ghostTotal;
    float scrLast;
    public Player player;



    //controla se o fantasma pode se mover ou não
    IEnumerator  SpeedControl()
    {

        //caso o jogador tiver se movendo, o fantasma se move tambem
        while (true)
        {
            if (player.totalPoint % 10 == 0 && player.totalPoint>0 && scrLast!=player.totalPoint)
            {
                scrLast = player.totalPoint;
                for (int i = 0; i < ghostTotal; i++)
                {
                    Debug.Log(Ghost[i].speed);
                    Ghost[i].speed *= 1.1f;
                    Debug.Log(Ghost[i].speed);
                }

            }
            yield return null;
        }
       
    }

    //procura o caminho para o fantasma chamado seguir o player 
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

    //realiza a troca de cor dos fantasmas para sinalizar que esta no final do scared state
    IEnumerator ChangeColorInFinalScaredState()
    {
        float timeToWait = Diff.ScaredStateTime[stage]/2;
        bool white = false;
        bool keepRun = true;
        float timeTotal = 0; 
        float time = 0;
        while (keepRun)
        {
            time = 0;
            while (time < timeToWait)
            {
                if (true)
                {
                    time += Time.deltaTime;
                    // Debug.Log(time);

                }

                yield return null;
            }        
            timeTotal += timeToWait;
            for (int i = 0; i < ghostTotal; i++)
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
           
        }
        
        
   
    }
   
    //coloca os fantasmas em modo assustado
    public void ScaredState()
    {
        
        MeshRenderer temp;

        for (int i = 0; i < ghostTotal; i++)
        {
            if (Ghost[i].gameObject.activeSelf)
            {
                temp = Ghost[i].GetComponentInChildren<MeshRenderer>();
                original[i] = temp.material.color;
                temp.material.SetColor("_Color", Color.blue);
                Ghost[i].speed = Diff.speed[stage][i] / 2;
                Ghost[i].destination = CornerPositon[i];
                if (!scareds[i])
                    StartCoroutine(FixCornerLock(i));
                scareds[i] = true;
            }
          
        }
        if (scaredRoutine == null)
        {
            ColorRoutine = StartCoroutine(ChangeColorInFinalScaredState());
            scaredRoutine = StartCoroutine(WaiScaredEnd());
        }
        else
        {
            StopCoroutine(ColorRoutine);
            StopCoroutine(scaredRoutine);
            ColorRoutine = StartCoroutine(ChangeColorInFinalScaredState());
            scaredRoutine = StartCoroutine(WaiScaredEnd());
        }
        


        
        
     
    }
    
    //controla o tempo que falta para acabar o modo assustado
    public IEnumerator WaiScaredEnd()
    {
        float time = 0;
        while (time < Diff.ScaredStateTime[stage])
        {
            if (true)
            {
                time += Time.deltaTime;
                // Debug.Log(time);

            }

            yield return null;
        }
        
        for (int i = 0; i < ghostTotal; i++)
            EndScareState(i);
        scaredRoutine = null;
        
    }

    //aumenta a dificulade
    public void NextStage()
    {
        
        stage += 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    //inicia os fantasmas
    IEnumerator StartG(int i)
    {
        float time = 0;
        while (time< Diff.startTime[stage][i])
        {
            if (true)
            {
                time += Time.deltaTime;
               // Debug.Log(time);

            }
               
            yield return null;
        }
        StartCoroutine(Corner(i));
        StartCoroutine(Refind(i));   
    }

    //ajusta a movimentacao dos fantansmas para que nao fiquem preso no canto caso o tempo de coner deles ainda nao
    //tenha acabado, mas ja tenha chegado no seu canto
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
    
    //controla quando mandar os fantasmas para o seu canto
    IEnumerator Corner(int i)
    {
        float time = 0;
        while (true)
        {
            time = 0;

            if (!scareds[i])
            {
                if (!isStarts[i])
                {
                   
                    while (time < Diff.cornerTime[stage][i])
                    {
                        if (true)
                        {
                            time += Time.deltaTime;
                            // Debug.Log(time);

                        }

                        yield return null;
                    }
                }           
                Ghost[i].destination = CornerPositon[i];
                backCorner[i] = true;
                StartCoroutine(FixCornerLock(i));
                isStarts[i] = false;
                time = 0;
                while (time < Diff.RefindAfterCorner[stage][i])
                {
                    if (true)
                    {
                        time += Time.deltaTime;
                        // Debug.Log(time);

                    }

                    yield return null;
                }
                backCorner[i] = false;
            }
            yield return null;

        }
            
    }

    //manda aleatoriamente um fantasma para o canto, mesmo que nao seja seu momento ainda(tentar aumentar a aleatoriedade
    //evitando que todos fiquem junto correndo atras do jogador)
    IEnumerator rndCorner()
    {
        float time = 0;
        while (true)
        {
            time = 0;
            while (time < Diff.rndCornerTime[stage])
            {
                if (true)
                {
                    time += Time.deltaTime;
                    // Debug.Log(time);

                }

                yield return null;
            }

             sort = Random.Range(0, ghostTotal);
        }
    }

    //controla o momento de recalcular a rota para perseguir o jogador
    IEnumerator Refind(int i)
    {
        float time = 0;
        while (true)
        {
            time = 0;
            while (time < Diff.refind)
            {
                if (true)
                {
                    time += Time.deltaTime;
                    // Debug.Log(time);

                }

                yield return null;
            }
            
           
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

    //revive o fantasma depois de ter sido morto no scared state
    public IEnumerator reviveGhost(GameObject ghost)
    {
        int i = 0;
        ghost.SetActive(false);        
        ghost.transform.position = new Vector3(-5.28f, 0.8332602f, -2.31f);
        float time = 0;
        while (time < Diff.timeToRespawn[stage])
        {
            if (true)
            {
                time += Time.deltaTime;
               // Debug.Log(time);

            }

            yield return null;
        }
        ghost.SetActive(true);
        while ((!GameObject.ReferenceEquals(ghost.GetComponent<NavMeshAgent>(), Ghost[i])))
            i++;
        EndScareState(i);



    }

    //acaba com o modo assustado dos fantasmas
    public void EndScareState(int i)
    {    
        MeshRenderer temp = Ghost[i].GetComponentInChildren<MeshRenderer>();
        temp.material.color = original[i];
        scareds[i] = false;
        Ghost[i].speed = Diff.speed[stage][i];
    }

    //prepara a incializacao dos fantasmas
    public void Prep_Stage()
    {
                 
        for (int i=0;i < ghostTotal; i++)
        {
            Ghost[i].gameObject.SetActive(true);

            Ghost[i].speed = Diff.speed[stage][i];
            StartCoroutine(StartG(i));
            StartCoroutine(SpeedControl());
                
        }
        //StartCoroutine(rndCorner());


    }

    //espera a pausa inicial do jogo para comecar
    public  IEnumerator StartEnemyIA()
    {
        
        while (GameStart.Pause)
        {
            yield return null;
        }
        Prep_Stage();
    }

    //setups iniciais
    private void Start()
    {
        
        
        Diff = stg.GetComponent<Stages>();
        ghostTotal = Diff.ghostActive[stage];
        Debug.Log(ghostTotal);
        StartCoroutine(StartEnemyIA());
        
    }

    void Update()
    {
        
        SpeedControl();
        
    }
}
