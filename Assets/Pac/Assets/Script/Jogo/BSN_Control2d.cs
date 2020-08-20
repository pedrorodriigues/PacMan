using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BSN_Control2d : MonoBehaviour
{
    public DataReceive data;
    public bool isFound = false;
    public bool isConnect = false;
    public bool isCaliber = false;

    //procura dispositivo
    public IEnumerator FindBSN()
    {

        while (BSNHardwareInterface.bsnDevice == null)
        {

            Debug.Log("xd");
            BSNHardwareInterface.FindBSN();

            yield return new WaitForSeconds(7);

        }
        yield return new WaitForSeconds(2);
        isFound = true;
    }

    //connecta pulseira
    public IEnumerator ConnectBSN()
    {
       
        BSNHardwareInterface.ConnectBSN();
        yield return new WaitForSeconds(35f);
        isConnect = true;


       

    }
    public IEnumerator CaliberBSN()
    {
        data.StartBsnData();
        while (!data.medFinish)
            yield return null;
        yield return new WaitForSeconds(2);
        isCaliber = true;

    }

}
