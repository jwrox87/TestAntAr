using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugInfo : MonoBehaviour
{

    Text textObj;

    [HideInInspector]
    public bool isTracking = false;

	// Use this for initialization
	void Awake () {

        textObj = GetComponent<Text>();       
    }

    void UpdateTrackinText()
    {
        if (isTracking)
        {
            textObj.color = Color.blue;
            textObj.text = "Tracking";
        }
        else
        {
            textObj.color = Color.red;
            textObj.text = "Not Tracking";
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateTrackinText();
    }
}
