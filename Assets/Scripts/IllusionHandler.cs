using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public enum IllusionStatus
{
    select, deployed
}

public class IllusionHandler : MonoBehaviour
{
    enum PlaneMode
    {
        GROUND,
        MIDAIR,
        PLACEMENT
    }

    public PlaneFinderBehaviour plane_finder;
    public GameObject Objs;
    //public GameObject Boy;
    //public GameObject Cat;
    //public GameObject Dog;

    public LivingObjManager livingObjManager;
    public UnityEngine.UI.Image select_image;
    public UnityEngine.UI.Image deployed_image;

    PositionalDeviceTracker m_PositionalDeviceTracker;
    GameObject planeAnchor;

    [HideInInspector]
    public string debugmsg = "";
    [HideInInspector]
    public string postrackermsg = "";

    IllusionStatus illusionStatus = IllusionStatus.select;

    Vector3 initialObjPosition;

    // Use this for initialization
    void Start ()
    {
        initialObjPosition = Objs.transform.position;

        Global.Instance.UICanvasManager.Toggle_UI_ByType(typeof(Toggle), false);

        livingObjManager.enabled = false;

        plane_finder.HitTestMode = HitTestMode.AUTOMATIC;

        TrackerManager.Instance.InitTracker<PositionalDeviceTracker>();
        m_PositionalDeviceTracker = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();

        StartCoroutine(ToggleStateCoroutine());
    }

    void RotateTowardCamera(GameObject augmentation)
    {
        var lookAtPosition = Camera.main.transform.position - augmentation.transform.position;
        lookAtPosition.y = 0;
        var rotation = Quaternion.LookRotation(lookAtPosition);
        augmentation.transform.rotation = rotation;
    }

    void DestroyAnchors()
    {
        Objs.transform.parent = null;

        if (planeAnchor)
        DestroyObject(planeAnchor);
    }

    void ResetAnchors()
    {
        if (m_PositionalDeviceTracker != null && m_PositionalDeviceTracker.IsActive)
        {
            m_PositionalDeviceTracker.Stop();
            m_PositionalDeviceTracker.Start();
        }
    }

    int anchorCount = 0;
    public void OnInteractiveHitTest(HitTestResult hitTestResult)
    {
        if (illusionStatus != IllusionStatus.select)
            return;

        if (m_PositionalDeviceTracker != null)
        {         
            DestroyAnchors();  

            planeAnchor = m_PositionalDeviceTracker.CreatePlaneAnchor("MyPlaneAnchor_" + (anchorCount++), hitTestResult);
            planeAnchor.name = "PlaneAnchor";

            if (!m_PositionalDeviceTracker.IsActive)
                m_PositionalDeviceTracker.Start();

            Objs.transform.SetParent(planeAnchor.transform);
            Objs.transform.localPosition = Vector3.zero;

            debugmsg = anchorCount.ToString();
        }

        //Make obj turn towards main camera
        RotateTowardCamera(Objs);

        //Place obj at location
        //Objs may need to be individual placed in the future
        Objs.PositionAt(hitTestResult.Position);

        //Set illusion status
        illusionStatus = IllusionStatus.deployed;
    }

    public void ToggleSelectIllusionStatus()
    {
        illusionStatus = IllusionStatus.select;
    }

    void ToggleStateLogic()
    {
        if (!deployed_image && !select_image)
            return;

        switch (illusionStatus)
        {
            case IllusionStatus.select:

                plane_finder.PlaneIndicator.SetActive(true);

                if (deployed_image.color == Color.gray)
                    return;

                deployed_image.color = Color.gray;
                select_image.color = Color.white;

                Global.Instance.UICanvasManager.Toggle_UI_ByType(typeof(Toggle), false);

                Global.Instance.UICanvasManager.Toggle_UI_ByName("ButtonDog", false);
                Global.Instance.UICanvasManager.Toggle_UI_ByName("ButtonBoy", false);
                Global.Instance.UICanvasManager.Toggle_UI_ByName("ButtonCat", false);

                Global.Instance.UICanvasManager.Toggle_UI_ByName("ButtonPlayDog", false);
                Global.Instance.UICanvasManager.Toggle_UI_ByName("ButtonPlayBoy", false);
                Global.Instance.UICanvasManager.Toggle_UI_ByName("ButtonPlayCat", false);

                livingObjManager.enabled = false;

                Objs.transform.position = initialObjPosition;

                break;

            case IllusionStatus.deployed:

                plane_finder.PlaneIndicator.SetActive(false);

                if (select_image.color == Color.gray)
                    return;

                select_image.color = Color.gray;
                deployed_image.color = Color.white;

                Global.Instance.UICanvasManager.Toggle_All_UI(true);

                livingObjManager.enabled = true;

                break;
        }
    }

    IEnumerator ToggleStateCoroutine()
    {
        while (true)
        {
            ToggleStateLogic();
            yield return new WaitForSeconds(0.5f);
        }
    }


    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            illusionStatus = IllusionStatus.deployed;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            illusionStatus = IllusionStatus.select;
        }
    }
    
    
}
