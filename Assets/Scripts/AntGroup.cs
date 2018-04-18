using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *  Ant group class. Does movement for a group of ants
 * 
 * 
 */
public class AntGroup : MonoBehaviour
{
    public float movePercentage = 0;

    public int path_id;

    [SerializeField]
    float walkSpeed = 1f;

    Transform[] checkPoints;

    public float WalkSpeed
    {
        get { return walkSpeed; }
    }
    public Transform [] CheckPoints
    {
        get { return checkPoints; }
    }

    void PathUpdate()
    {
        movePercentage = Mathf.Clamp(movePercentage, 0, 1);
        iTween.PutOnPath(this.gameObject, checkPoints, movePercentage);
    }

    public IEnumerator FirstRun()
    {
        yield return new WaitForSeconds(1f);

        StartCoroutine(MoveIncrement());
    }

    public IEnumerator MoveIncrement()
    {
        while (movePercentage < 1)
        {
            movePercentage += walkSpeed * Time.deltaTime;

            yield return new WaitForSeconds(0.03f);
        }
    }

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(FirstRun());
        path_id = Mathf.Clamp(path_id, 0, Global.Instance.Path_Finder.paths.Length - 1);
        checkPoints = Global.Instance.Path_Finder.paths[path_id].points;
	}
	
	// Update is called once per frame
	void Update ()
    {
        PathUpdate();
	}
}
