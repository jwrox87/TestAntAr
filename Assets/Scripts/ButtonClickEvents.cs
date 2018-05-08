using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClickEvents : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GoToAntScene()
    {
        SceneManager.LoadScene("ants_scene_AR_2");   
    }

    public void GoToSpeechScene()
    {
        SceneManager.LoadScene("speech_scene");
    }

}
