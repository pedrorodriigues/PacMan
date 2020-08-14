using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Comunication3 : MonoBehaviour {
	public GameObject eulerDebugger;
	public GameObject gravityDebugger;
    public GameObject quaternionsDebugger;
	public GameObject acelerometerDebugger;
	public GameObject cube;
	public GameObject resetButton;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeDebugger(){
		
		if(gravityDebugger.activeSelf){
			gravityDebugger.SetActive(false);
			eulerDebugger.SetActive(false);
			quaternionsDebugger.SetActive(true);
            acelerometerDebugger.SetActive(true);
            //cube.SetActive(false);
            // resetButton.SetActive(false);
		}else{
			if(quaternionsDebugger.activeSelf){
                gravityDebugger.SetActive(false);
                eulerDebugger.SetActive(false);
                quaternionsDebugger.SetActive(false);
                acelerometerDebugger.SetActive(false);
                //cube.SetActive(true);
                // resetButton.SetActive(true);
			}else{
                gravityDebugger.SetActive(true);
                eulerDebugger.SetActive(true);
                quaternionsDebugger.SetActive(false);
                acelerometerDebugger.SetActive(false);
                //cube.SetActive(false);
                // resetButton.SetActive(false);
			}
            
		}

	}
}
