using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Ant class in charge of 1 ant. Does its own rotation.
 * 
 * 
 */
public class Ant : MonoBehaviour
{
    public enum State
    {
        idle,
        walk
    }

    [SerializeField]
    float turnSpeed = 5f;

    [SerializeField]
    float animationSpeed = 1f;

    [SerializeField]
    float delayTurn = 0.01f;

    [SerializeField]
    State antState = State.walk;

    Animator ant_animator;

    AntGroup antgroup;

    int indexPt = 0;
    float movePercentage;
    int currentPath_id;

    public int CurrentPathId
    {
        get { return currentPath_id; }
        set { currentPath_id = CurrentPathId; }
    }
    public int CurrentIndexPt
    {
        get { return indexPt; }
    }

    public IEnumerator FirstRun()
    {
        yield return new WaitForSeconds(1f);

        StartCoroutine(RotateIncrement());
    }

    public IEnumerator RotateIncrement()
    {
        while (movePercentage < 1)
        {
            StartCoroutine(RotateBody());

            yield return new WaitForSeconds(delayTurn);
        }
    }

    public IEnumerator RotateBody()
    {
        if (indexPt + 1 < antgroup.CheckPoints.Count)
            this.CorrectFacing(antgroup.CheckPoints[indexPt + 1].position - antgroup.CheckPoints[indexPt].position);

        yield return new WaitForSeconds(0.01f);
    }

    void AntState()
    {
        switch (antState)
        {
            case State.idle:
                ant_animator.Play("idle");
                break;

            case State.walk:
                ant_animator.Play("walk");
                break;
        }
    }

    public void Initialize()
    {
        ant_animator = GetComponent<Animator>();
        antgroup = this.transform.parent.GetComponent<AntGroup>();

        ant_animator.speed = animationSpeed;

        currentPath_id = antgroup.path_id;

        AntState();
    }

    public void ResetIndex(int resetValue)
    {
        indexPt = resetValue;
    }

    public void DetectSurface()
    {
        RaycastHit hit;
        
        Vector3 dir = -transform.up;

        float posDetectSpeed = 10f;
        float rotDetectSpeed = 10f;

        var ray = new Ray(transform.position, dir);
        Debug.DrawRay(transform.position, dir * 5f);

        if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
        {
            Vector3 pt = hit.point + hit.normal * 0.5f;
            transform.position = Vector3.Lerp(transform.position, pt, Time.deltaTime * posDetectSpeed);

            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation,
                Time.deltaTime * rotDetectSpeed);
        }
    }

    public void CorrectFacing(Vector3 target)
    {
        Quaternion target_rot =
            Quaternion.FromToRotation(transform.right, target.normalized) * transform.rotation;

        transform.rotation = Quaternion.Lerp(transform.rotation, target_rot, Time.deltaTime * turnSpeed);
       
    }

    void CheckCurrentPoint()
    {
        if (indexPt + 1 >= antgroup.CheckPoints.Count)
            return;

        float dist = Vector3.Distance(this.transform.parent.position, antgroup.CheckPoints[indexPt + 1].position);

        if (dist < 0.5f)
        {
            indexPt += 1;
            return;
        }
    }

    public void MoveUpdate()
    {
        movePercentage = antgroup.movePercentage;

        AntState();

        if (movePercentage > 0 && movePercentage < 1)
            antState = State.walk;
        else      
            antState = State.idle;

        CheckCurrentPoint();
    }

}
