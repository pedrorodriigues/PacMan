using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonVR : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        
    }
    IEnumerator WaitOnGazeEnterToStart()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GazeEnter()
    {
        StartCoroutine(WaitOnGazeEnterToStart());
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
