using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct randomNumbers
{
    public float randomSpeed;
    public float randomX, randomY, randomZ;
}
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

    randomNumbers randomVals;
    Vector3 initialPos;

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
            StartCoroutine(RandomValues());
            yield return new WaitForSeconds(delayTurn);
        }
    }

    public IEnumerator RotateBody()
    {
        if (indexPt + 1 < antgroup.CheckPoints.Count)
        {
            Vector3 diff = antgroup.CheckPoints[indexPt + 1].position - antgroup.CheckPoints[indexPt].position;
            this.CorrectFacing(diff);
        }

        yield return new WaitForSeconds(0.01f);
    }


    public IEnumerator RandomValues()
    {
        randomVals.randomSpeed = ExtensionMethods<float>.Randomize(10f, 15f);

        yield return new WaitForSeconds(0.5f);
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

        initialPos = transform.localPosition;
        
        randomVals.randomX = ExtensionMethods<float>.Randomize(10f, 15f);
        randomVals.randomY = ExtensionMethods<float>.Randomize(10f, 15f);
        randomVals.randomZ = ExtensionMethods<float>.Randomize(10f, 15f);
    }

    public void ResetIndex(int resetValue)
    {
        indexPt = resetValue;
    }

    public void DetectSurface()
    {
        RaycastHit hit;      
        Vector3 dir = -transform.up;
        float rotDetectSpeed = 10f;

        Debug.DrawRay(transform.position, dir);

        var ray = new Ray(transform.position, dir);

        if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
        {
            Vector3 destination = (antgroup.transform.InverseTransformPoint(hit.point) + hit.normal * 0.5f);

            float x = Mathf.Clamp(destination.x, initialPos.x - randomVals.randomX, initialPos.x + randomVals.randomX);
            float y = Mathf.Clamp(destination.y, initialPos.y - randomVals.randomY, initialPos.y + randomVals.randomY);
            float z = Mathf.Clamp(destination.z, initialPos.z - randomVals.randomZ, initialPos.z + randomVals.randomZ);

            transform.localPosition = Vector3.Lerp(transform.localPosition,
                new Vector3(x, y, z), Time.smoothDeltaTime * randomVals.randomSpeed);

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
