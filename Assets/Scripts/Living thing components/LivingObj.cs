using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum State
{
    idle,
    move
};

public class LivingObj : MonoBehaviour
{
    private float pointOnPath;
    private int checkPt = 1;

    [SerializeField]
    protected float walkSpeed;

    [SerializeField]
    protected Transform walkPath;

    State currentstate = State.idle;

    public State CurrentState
    {
        get { return currentstate; }
        set { currentstate = value; }
    }

    protected LivingObj()
    {
        walkSpeed = 1f;
    }

    protected LivingObj(float ws)
    {
        walkSpeed = ws;
    }

    public float GetWalkSpeed()
    {
        return walkSpeed;
    }

    public Transform[] GetWalkPath()
    {
        var childrenList 
            = walkPath.GetComponentsInChildren<Transform>(true).
            Where(x => x.transform.parent == walkPath.transform).ToArray();

        return childrenList;
    }

    public float PointOnPath
    {
        get { return pointOnPath; }
        set { pointOnPath = value; }
    }

    public int CheckPoint
    {
        get { return checkPt; }
        set { checkPt = value; }
    }

    public Animator GetAnimatorComponent()
    {
        if (!GetComponent<Animator>())
            return null;

        return GetComponent<Animator>();
    }

    public void CrossFadeTo(string animation_name)
    {
        if (!GetAnimatorComponent().GetCurrentAnimatorStateInfo(0).IsName(animation_name))
        {
            GetAnimatorComponent().CrossFade(animation_name, 0);
        }
    }


}
