using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pontuacao : MonoBehaviour
{
    
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "PacPoint")
        {
            Debug.Log("xD");
            Destroy(col.gameObject);
        }

    }
   

}
