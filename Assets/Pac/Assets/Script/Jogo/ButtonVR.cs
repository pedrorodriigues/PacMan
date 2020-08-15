using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonVR : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider loadBar;
    void Start()
    {
        
    }

    IEnumerator WaitOnGazeEnterToStart(int opt)
    {
        loadBar.gameObject.SetActive(true);
        float time=0;
        while (time<3)
        {
            time += Time.deltaTime;
            loadBar.value = time / 3;
            yield return null;
        }
        if (opt == 1)
            StartGame();
        else
            QuitGame();
    }

    public void StartGame()
    {
        int valueScene = 1;
        Debug.Log("testando nome aqui" + this.gameObject.tag);
        if (this.gameObject.tag == "final")
            valueScene = -1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + valueScene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void GazeEnterPlay(int opt)
    {       
        StartCoroutine(WaitOnGazeEnterToStart(opt));
    }

    public void GazeExit()
    {

        StopAllCoroutines();
        loadBar.gameObject.SetActive(false);
        loadBar.value = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
