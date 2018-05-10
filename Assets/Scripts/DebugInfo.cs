using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInfo : MonoBehaviour
{

    TextMesh textmesh;

    public bool isTracking = false;
    public bool SpeechBubbleManagerPresent = false;
    public bool hasEnterCoroutine = false;

	// Use this for initialization
	void Start () {

        textmesh = GetComponent<TextMesh>();
        
    }

    public void AddMsg(string s)
    {
     
    }
	
	// Update is called once per frame
	void Update ()
    {
        textmesh.text = "Tracking: " + isTracking + '\n' +
                        "Speech Bubble Manager: " + SpeechBubbleManagerPresent + '\n' +
                        "hasEnterCoroutine: " + hasEnterCoroutine;
    }
}
