using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityStandardAssets.Characters.FirstPerson;

public class DataReceive : MonoBehaviour
{
    // public TextMeshProUGUI x1;
    //public TextMeshProUGUI x2;
    // public TextMeshProUGUI x3;
    // public TextMeshProUGUI x4;
    //public TextMeshProUGUI x5;
    // public TextMeshProUGUI x6;
    public bool isEditor=false;
    private double[] gravity = new double[3];
    private double[] preValueGravity = new double[3];
    private double yMed;  
    public static bool press = false;
    bool walked=false;
    int nInt = 100;
    public bool medFinish = false;
    public float time = 0;
    //alterar dependendo da velocidade do passo da pessoa : menor/manter se for muiito rapido : aumentar se for lento
    public float intervalToPrev = 0.15f;
    //deminuir caso levante muito pouco a perna
    public float maxDiffToStep = 0.2f;
    //pouca necessidade de mudar, mas pode reduzir caso esteja captando muito passo extra, mas alterar os 2 antes deve trazer melhor resultado
    public float maxDiffToResetStep = 0.08f;

    //pega o valor medio de 10 marcações do valor y do vetor gravidade
    IEnumerator GetGravityYMed()
    {
        double aux=0;
        yield return new WaitForSeconds(1f); 
        int i=0;
        while (i < nInt)
        {
            aux += gravity[1];
            i++;
            yield return null;
            
        }
        aux = aux / nInt;
        yMed = aux;
        StartCoroutine(SterpVer());
        medFinish = true;
       
    }

   
    public void StartBsnData()
    {
        gravity = BSNHardwareInterface.ReceiveGravityVector();
        preValueGravity[1] = 0;
        StartCoroutine(GetGravityYMed());
    }

   
    //responsavel por verificar se ouve passo
    IEnumerator SterpVer()
    {
        
        

        while (true) { 
            time += Time.deltaTime;
            
            if (time > intervalToPrev)
            {
                //verifica se a diferenca dso valores em um intervalo de tempo é suficiente para registrar um passo e se
                //um passo ja n foi dado antes de a perna ter voltado para o tepouso
                if (Math.Abs(gravity[1] - preValueGravity[1]) >= maxDiffToStep && !walked)
                {
                    walked = true;
                    press = true;
                }
                else
                {
                    preValueGravity[1] = gravity[1];
                    time = 0;
                }
            }
            //caso a perna tenha voltado para repouso, libera para ser possivel dar outro passo
            if (Math.Abs(gravity[1]-yMed)< maxDiffToResetStep && walked)
            {
                preValueGravity[1] = gravity[1];
                walked = false;
            }
            
            yield return null;
        }

    }

    //fucao para testar no editor
    private void Update()
    {
        if (isEditor)
            if (Input.GetMouseButton(0))
                press = true;
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
