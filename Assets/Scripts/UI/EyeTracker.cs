using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyeTracker : MonoBehaviour {

    Image image;
    DefaultTrackableEventHandler[] defaultTrackable;

    // Use this for initialization
    void Start () {

        image = transform.GetComponent<Image>();

        defaultTrackable = GameObject.FindObjectsOfType<DefaultTrackableEventHandler>();
    }

    public void ToggleEye(bool b)
    {
        image.enabled = b;
    }


    bool CheckTracking()
    {
        foreach (DefaultTrackableEventHandler df in defaultTrackable)
        {
            if (df.IsTracking)
                return true;
        }

        return false;
    }
	
	// Update is called once per frame
	void Update () {

        if (defaultTrackable.Length > 0)
        ToggleEye(CheckTracking());
      
	}
}
