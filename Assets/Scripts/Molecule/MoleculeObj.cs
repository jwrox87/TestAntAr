using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeObj : MonoBehaviour
{
    public MoleculeHandler handler;
    public Transform placement;
    public Transform indicator;

    public MoleculeState moleculeState = MoleculeState.idle;
    public MoleculeType moleculeType = MoleculeType.diamond;

    Renderer moleculeRender;

	// Use this for initialization
	void Start ()
    {
        moleculeRender = GetComponent<Renderer>();
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
            throw new System.Exception("No indicator on " + this.name);

        if (!up)
            indicator.localEulerAngles = new Vector2(0, 180);
        else
            indicator.localEulerAngles = new Vector2(0, 0);
    }

}
