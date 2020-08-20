using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BSN_Control : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject preMenu;
    public GameObject Menu;
    //public TextMeshProUGUI deviceFound;
    //public TextMeshProUGUI deviceCon;
    //public TextMeshProUGUI caliber;
    public DataReceive data;

    //procura dispositivo
    IEnumerator StartFind()
    {
        
        while (BSNHardwareInterface.bsnDevice == null)
        {
           
            BSNHardwareInterface.FindBSN();
            
            yield return new WaitForSeconds(7);

        }
        //deviceFound.text = "Dispositivo Encontrado";
       StartCoroutine(Connect());
    }
    
    //connecta pulseira,ativa/desativas os menus e chama para calibrar
    IEnumerator Connect()
    {
       // deviceCon.enabled = true;
        BSNHardwareInterface.ConnectBSN();
        yield return new WaitForSeconds(35f);
        //deviceCon.text = "Conectado";
        data.StartBsnData();
        //caliber.enabled = true;
        while (!data.medFinish)
            yield return null;
       // caliber.text="calibrado";
        yield return new WaitForSeconds(2);
        preMenu.SetActive(false);
        Menu.SetActive(true);


    }
    void Start()
    {
        StartCoroutine(StartFind());      
    }

}
