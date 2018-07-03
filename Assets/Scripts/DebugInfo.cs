using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FaceTrackerExample;
using Vuforia;

public class DebugInfo : MonoBehaviour
{

    Text textObj;

    public bool tracker = true;

    [HideInInspector]
    public bool isTracking = false;

    public FaceTrackerARExampleTest refObj;
    public IllusionHandler refIH;

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

    void UpdateResolution()
    {
        if (!refObj)
            return;

        //textObj.text = "Width: " +refObj.requestedWidth + "\n"
        //    + "Height: " +refObj.requestedHeight;


        textObj.text = "Width: " + Screen.width
            + " \n Height: " + Screen.height
            + "\n Camera Size: " + Vuforia.VuforiaConfiguration.Instance.Vuforia.CameraDirection
            + "\n Face Rect: " + Camera.main.WorldToScreenPoint(Global.Instance.SpeechBubble_Manager.Face_Rect_Pos)
            + "\n Touch Pos: " + Input.mousePosition;

    }

    void UpdateIllusion()
    {
        if (!refIH)
            return;

        textObj.text = "Click position: " + refIH.debugmsg
            + "\n Position tracker: " + refIH.postrackermsg
            ;
    }


    void SimpleMsg()
    {
        textObj.text = VuforiaBehaviour.Instance.enabled.ToString();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!textObj)
            return;

        if (tracker)
            UpdateTrackinText();
        else
        {
            UpdateResolution();
            UpdateIllusion();
        }

        //SimpleMsg();

    }
}
