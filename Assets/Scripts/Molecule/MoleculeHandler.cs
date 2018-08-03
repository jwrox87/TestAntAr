using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum MoleculeState
{
    idle,
    inserted
}

public enum MoleculeType
{
    diamond,
    square
}

public class MoleculeHandler : MonoBehaviour
{
    //Can only take 1 molecule at a time
    public MoleculeObj current_molecule { get; set; }

    MoleculeType accepted_moleculeType = MoleculeType.diamond;

    public Transform placeholder;
    public Transform success_target;
    public Transform failure_target;

    public MoleculeMsgHandler msgHandler;

    delegate void MoleculeEvent();
    MoleculeEvent Current_Event;

    DefaultTrackableEventHandler surface_defaultTrackable;

    // Use this for initialization
    void Start()
    {
        surface_defaultTrackable = placeholder.root.GetComponent<DefaultTrackableEventHandler>();

        if (!msgHandler)
            throw new Exception("No msg handler found");
    }

    float GetDistanceBetweenTargets(Vector3 pos, Vector3 target)
    {
        return Vector3.Distance(pos, target);
    }

    WaitForSeconds initialdelay = new WaitForSeconds(0.5f);
    IEnumerator InsertMolecule()
    {
        placeholder.gameObject.SetActive(false);

        current_molecule.SetRendererState(true);

        current_molecule.transform.parent = success_target.root;
        //current_molecule.transform.position = placeholder.position;
        current_molecule.transform.localEulerAngles = success_target.localEulerAngles;
        
        yield return initialdelay;

        Transform target;
        if (accepted_moleculeType == current_molecule.moleculeType)
        {
            target = success_target;
            msgHandler.ShowMsg(MoleculeMsgHandler.MsgState.correct, true);
        }
        else
        {
            target = failure_target;
            msgHandler.ShowMsg(MoleculeMsgHandler.MsgState.wrong, true);
        }

        while (GetDistanceBetweenTargets(current_molecule.transform.position, target.position) > 0.1f)
        {
            current_molecule.transform.position = Vector3.Lerp(current_molecule.transform.position, target.position, Time.deltaTime * 2f);
            yield return null;
        }

        current_molecule.transform.position = target.position;
        current_molecule.transform.localEulerAngles = target.localEulerAngles;

        current_molecule.moleculeState = MoleculeState.inserted;

        current_molecule.ToggleIndicator(true);

        msgHandler.ShowMsg(false);
    }

    IEnumerator RemoveMolecule()
    {
        yield return initialdelay;

        current_molecule.transform.parent = current_molecule.placement.parent;

        while (GetDistanceBetweenTargets(current_molecule.transform.position, current_molecule.placement.position) > 0.1f)
        {
            current_molecule.transform.position = Vector3.Lerp(current_molecule.transform.position,
                current_molecule.placement.position, Time.deltaTime * 4f);
            yield return null;
        }

        //If user hides image target at last minute, make current molecule disappear
        if (!current_molecule.defaultTrackable.IsTracking)
            current_molecule.SetRendererState(false);

        current_molecule.transform.position = current_molecule.placement.position;
        current_molecule.transform.localEulerAngles = current_molecule.placement.localEulerAngles;

        placeholder.gameObject.SetActive(true);

        current_molecule.moleculeState = MoleculeState.idle;

        current_molecule.ToggleIndicator(false);

    }

    bool is_Insert = false;
    void Event_InsertMolecule()
    {
        if (!is_Insert)
        {
            is_Insert = true;
            StartCoroutine(InsertMolecule());
        }
    }

    void Event_RemoveMolecule()
    {
        if (is_Insert)
        {
            is_Insert = false;
            StartCoroutine(RemoveMolecule());
        }
    }


    void Event_Handler()
    {
        if (!current_molecule)
            return;

        switch (current_molecule.moleculeState)
        {
            case MoleculeState.idle:

                if (GetDistanceBetweenTargets(current_molecule.transform.position, placeholder.position) < 10f)
                    Current_Event = Event_InsertMolecule;

                break;

            case MoleculeState.inserted:

                if (!surface_defaultTrackable.IsTracking)
                    return;

                if (current_molecule.defaultTrackable.IsTracking)
                    Current_Event = Event_RemoveMolecule;

                break;
        }

        if (Current_Event != null)
            Current_Event();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Event_Handler();
    }
}
