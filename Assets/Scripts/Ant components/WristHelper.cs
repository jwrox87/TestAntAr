using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Purpose of this class is to make a quick option to view the wrist obj
/// Able to view the wrist obj would allow user to align their arm with the obj
/// </summary>
public class WristHelper : MonoBehaviour
{
    public Slider alphaSlider;
    public GameObject wristMat_obj;

    Material wristMat_mat;
    Color wristMat_color;

	// Use this for initialization
	void Start ()
    {
        wristMat_mat = wristMat_obj.GetComponent<Renderer>().material;
        wristMat_color = wristMat_mat.color;

        alphaSlider.gameObject.SetActive(false);
        wristMat_mat.color = new Color(wristMat_color.r, wristMat_color.g, wristMat_color.b,0f);
    }

    void ControlWristColor()
    {
        float val = alphaSlider.value;

        wristMat_mat.color = new Color(wristMat_color.r, wristMat_color.g, wristMat_color.b,
            val);
    }


    void ToggleAlphaSlider()
    {
        //alphaSlider.gameObject.SetActive(defaultTrackable.IsTracking);
    }
	
	// Update is called once per frame
	void Update ()
    {
        ToggleAlphaSlider();

        //if (defaultTrackable.IsTracking)
        // ControlWristColor();
	}
}
