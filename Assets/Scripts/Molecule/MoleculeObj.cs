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

    public DefaultTrackableEventHandler defaultTrackable { get; set; }

	// Use this for initialization
	void Start ()
    {
        moleculeRender = GetComponent<Renderer>();
        defaultTrackable = transform.parent.GetComponent<DefaultTrackableEventHandler>();
    }

    void OnTriggerEnter(Collider col)
    {
       if (col.name == "Placeholder")
        {
            handler.current_molecule = this;
        }
    }

    public void SetRendererState(bool b)
    {
        moleculeRender.enabled = b;
    }


    public void ToggleIndicator(bool up)
    {
        if (!indicator)
            throw new System.Exception("No indicator on " + this.name);

        if (!up)
            indicator.localEulerAngles = new Vector2(0, 180);
        else
            indicator.localEulerAngles = new Vector2(0, 0);
    }

}
