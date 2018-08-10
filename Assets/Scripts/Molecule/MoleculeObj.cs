using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeObj : MonoBehaviour
{
    public MoleculeHandler handler;
    public Transform placement;
    public Transform indicator;

    [HideInInspector]
    public MoleculeState moleculeState = MoleculeState.idle;
    public MoleculeType moleculeType = MoleculeType.diamond;

    Renderer moleculeRender;
    SpringJoint springJoint;
    Rigidbody rigidBody;
    Rigidbody anchorRigidBody;

    public Vector3 initialRotation { get; set; }
    public DefaultTrackableEventHandler defaultTrackable { get; set; }
    public IEnumerator enumerator { get; set; }

	// Use this for initialization
	void Start ()
    {
        moleculeRender = GetComponent<Renderer>();
        springJoint = GetComponent<SpringJoint>();
        rigidBody = GetComponent<Rigidbody>();

        anchorRigidBody = springJoint.connectedBody;

        defaultTrackable = anchorRigidBody.transform.parent.GetComponent<DefaultTrackableEventHandler>();
        initialRotation = transform.localEulerAngles;

        indicator.gameObject.SetActive(false);

        enumerator = Hide();
        StartCoroutine(enumerator);
    }


    public void SetKinematic(bool b)
    {
        rigidBody.isKinematic = b;
    }

    public void SetRendererState(bool b)
    {
        moleculeRender.enabled = b;
    }


    bool toggleReset = false;
    IEnumerator Hide()
    {
        while (true)
        {
            if (!defaultTrackable.IsTracking)
            {
                SetKinematic(true);
                SetRendererState(false);

                toggleReset = false;
            }
            else
            {
                if (!toggleReset)
                {
                    transform.position = placement.position;
                    toggleReset = true;
                }

                SetKinematic(false);
                SetRendererState(true);
            }

            yield return null;
        }
    }


    void OnTriggerEnter(Collider col)
    {
       if (col.name == "Placeholder")
        {
            handler.current_molecule = this;
        }
    }


    public void ToggleIndicator(bool up)
    {
        if (!indicator)
            return;

        if (!up)
            indicator.localEulerAngles = new Vector2(0, 180);
        else
            indicator.localEulerAngles = new Vector2(0, 0);
    }

}
