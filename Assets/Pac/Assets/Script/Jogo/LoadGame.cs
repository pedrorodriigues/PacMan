using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class LoadGame : MonoBehaviour
{

    public void LoadNext()
    {
        StartCoroutine(activeVR());
        
    }

    IEnumerator activeVR()
    {

        XRSettings.LoadDeviceByName("Cardboard");
        yield return null;
        XRSettings.enabled = true;
        SceneManager.LoadScene(1);
    }
   


}
