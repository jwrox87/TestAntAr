﻿using System.Collections;
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

    Ant[] OwnGroupAnts;

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

    void Loop()
    {
        if (movePercentage >= 1 
            && Global.Instance.Path_Finder.paths[path_id].type == Path.Type.Loop)
        {
            Path p = Global.Instance.Path_Finder.paths[path_id];

            p.points[0] = p.points[p.points.Length - 1];
            p.points[1] = p.loopPoint;
            movePercentage = 0;
            ResetAllAnts(0);
        }
    }

    void ResetAllAnts(int resetIndexValue)
    {
        foreach (Ant ant in OwnGroupAnts)
        {
            ant.ResetIndex(resetIndexValue);
        }
    }

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(FirstRun());
        path_id = Mathf.Clamp(path_id, 0, Global.Instance.Path_Finder.paths.Length - 1);
        checkPoints = Global.Instance.Path_Finder.paths[path_id].points;

        OwnGroupAnts = transform.GetComponentsInChildren<Ant>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        PathUpdate();
        Loop();
	}
}
