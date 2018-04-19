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

    Transform []checkPoints;
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
        if (indexPt + 1 < checkPoints.Length)
            this.CorrectFacing(checkPoints[indexPt + 1].position - checkPoints[indexPt].position);

        // this.CorrectFacing(antgroup.transform.position - transform.position);
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

        checkPoints = Global.Instance.Path_Finder.paths[currentPath_id].points;

        AntState();
    }

    public void ResetIndex(int resetValue)
    {
        indexPt = resetValue;
    }

    void MapToSurface(float distance)
    {
        var distanceToGround = distance;
        var currPos = transform.position;
        var newY = (currPos.y - distanceToGround) + transform.localScale.y * 6f;

        transform.position = new Vector3(currPos.x, newY, currPos.z);
    }

    public void DetectSurface()
    {
        RaycastHit hit;
        var ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray,out hit, 5f))
        {
            MapToSurface(hit.distance);

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
    }

    public void CorrectFacing(Vector3 target)
    {
        Quaternion target_rot =
            Quaternion.FromToRotation(transform.right, target.normalized) * transform.rotation;

        transform.rotation = Quaternion.Lerp(transform.rotation,target_rot, Time.deltaTime * turnSpeed);
    }

    void CheckCurrentPoint()
    {
        if (indexPt + 1 >= checkPoints.Length)
            return;

       float dist = Vector3.Distance(this.transform.parent.position, checkPoints[indexPt + 1].position);

        //float dist = checkPoints[indexPt + 1].position.x - this.transform.position.x;
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
        {
            antState = State.walk;
        }
        else
        {
            antState = State.idle;
        }

        CheckCurrentPoint();
    }

}
