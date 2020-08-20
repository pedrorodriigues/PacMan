using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class PreGameStartMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Menu;
    public BSN_Control2d bsn;
    public LoadGame load;
    public GameObject menuP1,menuP2,menuP3;
    public GameObject P1wait,p1finish, P2wait, p2finish, P3wait, p3finish,bt1,bt2;


    public void openMenu()
    {
        Menu.SetActive(false);
        this.gameObject.SetActive(true);
        bsn.StartCoroutine(bsn.FindBSN());
    }
    IEnumerator Call3dMenu()
    {
        yield return new WaitForSeconds(2);
        load.LoadNext();
    }

    // Update is called once per frame
    void Update()
    {
        if (bsn.isFound)
        {
            P1wait.SetActive(false);
            p1finish.SetActive(true);
            menuP2.SetActive(true);
            bsn.StartCoroutine(bsn.ConnectBSN());
            bsn.isFound = false;
        }
        if (bsn.isConnect)
        {
            P2wait.SetActive(false);
            p2finish.SetActive(true);
            menuP3.SetActive(true);
            bsn.StartCoroutine(bsn.CaliberBSN());
            bsn.isConnect = false;
        }
        if (bsn.isCaliber)
        {
            P3wait.SetActive(false);
            p3finish.SetActive(true);  
            bsn.isCaliber = false;
            StartCoroutine(Call3dMenu());
            
        }
    }
}
