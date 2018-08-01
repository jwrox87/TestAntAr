using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoleculeState
{
    idle,
    inserted
}

class Molecule
{
    string name;

}

public class MoleculeHandler : MonoBehaviour
{
    public Transform square_molecule;
    public Transform square_molecule_placement;
    public Transform placeholder;
    public Transform destination;

    MoleculeState moleculeState = MoleculeState.idle;

    delegate void MoleculeEvent();
    MoleculeEvent Current_Event;

    // Use this for initialization
    void Start()
    {
       
    }

    float GetDistanceBetweenTargets(Vector3 pos, Vector3 target)
    {
        return Vector3.Distance(pos, target);
    }

    WaitForSeconds initialdelay = new WaitForSeconds(0.5f);
    IEnumerator InsertMolecule()
    {
        placeholder.gameObject.SetActive(false);

        square_molecule.parent = destination.parent.parent;
        square_molecule.position = placeholder.position;
        square_molecule.localEulerAngles = destination.localEulerAngles;
        
        yield return initialdelay;

        while (GetDistanceBetweenTargets(square_molecule.position,destination.position) > 0.1f)
        {
            square_molecule.position = Vector3.Lerp(square_molecule.position, destination.position, Time.deltaTime * 2f);
            yield return null;
        }

        square_molecule.position = destination.position;
        square_molecule.localEulerAngles = destination.localEulerAngles;

        moleculeState = MoleculeState.inserted;

    }

    IEnumerator RemoveMolecule()
    {
        yield return initialdelay;

        square_molecule.parent = square_molecule_placement.parent;

        while (GetDistanceBetweenTargets(square_molecule.position,square_molecule_placement.position) > 0.1f)
        {
            square_molecule.position = Vector3.Lerp(square_molecule.position, square_molecule_placement.position, Time.deltaTime * 2f);
            yield return null;
        }

        square_molecule.position = square_molecule_placement.position;
        square_molecule.localEulerAngles = square_molecule_placement.localEulerAngles;

        placeholder.gameObject.SetActive(true);

        moleculeState = MoleculeState.idle;
    }

    bool is_Insert = false;
    void Event_InsertMolecule()
    {
        if (!is_Insert)
        {
            is_Remove = false;
            is_Insert = true;

            StartCoroutine(InsertMolecule());
        }
    }

    bool is_Remove = false;
    void Event_RemoveMolecule()
    {
        if (!is_Remove)
        {
            is_Insert = false;
            is_Remove = true;

            StartCoroutine(RemoveMolecule());
        }
    }

    void Event_Handler()
    {
        if (moleculeState == MoleculeState.idle)
        {
            if (GetDistanceBetweenTargets(square_molecule.position, placeholder.position) < 10f)
                Current_Event = Event_InsertMolecule;
        }
        else
        {
            if (GetDistanceBetweenTargets(square_molecule_placement.position, square_molecule.position) < 20f)
                Current_Event = Event_RemoveMolecule;
        }

        if (Current_Event != null)
            Current_Event();
    }

    // Update is called once per frame
    void Update()
    {
        Event_Handler();
    }
}
