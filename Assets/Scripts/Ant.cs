using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    float walkSpeed = 1f;

    [SerializeField]
    State antState = State.walk;

    Transform []checkPoints;

    Animator ant_animator;

    public float WalkSpeed
    {
        get { return walkSpeed; }
    }

    public int indexPt = 0;
    public float movePercentage = 0;

    public int currentPath_id;

    public IEnumerator FirstRun()
    {
        yield return new WaitForSeconds(1f);

        StartCoroutine(MoveIncrement());
    }

    public IEnumerator RotateBody()
    {
        if (indexPt + 1 < checkPoints.Length)
            this.CorrectFacing(checkPoints[indexPt + 1].position - checkPoints[indexPt].position);

        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator MoveIncrement()
    {
        while (movePercentage < 1)
        {
            movePercentage += walkSpeed * Time.deltaTime;

            StartCoroutine(RotateBody());

            yield return new WaitForSeconds(0.03f);
        }
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

        ant_animator.speed = animationSpeed;

        checkPoints = Global.Instance.Path_Finder.paths[currentPath_id].points;

        AntState();
    }

    public void DetectSurface()
    {
        RaycastHit hit;
        var ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray,out hit, 5f))
        {
            var slope = hit.normal;

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

        float dist = Vector3.Distance(transform.position, checkPoints[indexPt + 1].position);

        if (dist < 0.5f)
        {
            indexPt += 1;
            return;
        }
    }

    void PathUpdate()
    {
        movePercentage = Mathf.Clamp(movePercentage, 0, 1);
        iTween.PutOnPath(this.gameObject, checkPoints, movePercentage);
    }

    public void MoveUpdate()
    {
        CheckCurrentPoint();
        PathUpdate();
    }

}
