using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyeTracker : MonoBehaviour {

    Image image;

	// Use this for initialization
	void Start () {

        image = transform.GetComponent<Image>();
    }

    public void ToggleEye(bool b)
    {
        image.enabled = b;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
