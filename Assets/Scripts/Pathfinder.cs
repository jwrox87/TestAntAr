using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Path
{
    public enum Type
    {
        Linear, Loop
    }

    public List<Transform> points;
    public List<Transform> points2;

    public Color color = new Color(1, 1, 1, 1);

    public Type type = Type.Linear;

    List<Transform> cachedPoints;
    Dictionary<int,Transform> pointsOfIntersection;

    public Dictionary<int,Transform> PointsOfIntersection
    {
        get { return pointsOfIntersection; }
    }


    public void Init()
    {
        cachedPoints = points;
        InitIntersectionPts();
    }

    public void InitIntersectionPts()
    {
        pointsOfIntersection = new Dictionary<int, Transform>();

        for (int i = 0; i < points.Count; i++)
        {
            foreach (Transform t2 in points2)
            {
                if (points[i].localPosition == t2.localPosition)
                    pointsOfIntersection.Add(i, points[i]);
            }
        }
    }

    public void Switch(List<Transform> p)
    {
        points = p;
    }

    bool initLoop = false;
    public void InitLoop()
    {
        if (initLoop)
            return;

        points[0].localPosition = points[points.Count - 1].localPosition;

        if (points2.Count > 2)
        {
            points2[0].localPosition = points2[points2.Count - 1].localPosition;
        }

        initLoop = true;
    }

    public void Skip(int skipIndex)
    {
        points.Remove(points[skipIndex]);   
    }

    public void Reset()
    {
        points = cachedPoints;
    }

}

public class Pathfinder : MonoBehaviour
{
    [SerializeField]
    public Path[] paths;

    // Use this for initialization
    void Start()
    {
        for (int x = 0; x < paths.Length; x++)
        {
            for (int i = 0, j = 1; i < paths[x].points.Count - 1; i++, j++)
            {
                paths[x].Init();
            }
        }
    }

    void OnDrawGizmos()
    {
        //for (int x = 0; x < paths.Length; x++)
        //{
        //    iTween.DrawPathGizmos(paths[x].points,paths[x].color);         
        //}

        for (int x = 0; x < paths.Length; x++)
        {
            for (int i = 0, j = 1; i < paths[x].points.Count - 1; i++, j++)
            {
                Gizmos.color = paths[x].color;
                Gizmos.DrawLine(paths[x].points[i].position, paths[x].points[j].position);
            }

            for (int i = 0, j = 1; i < paths[x].points2.Count - 1; i++, j++)
            {
                Gizmos.color = paths[x].color;
                Gizmos.DrawLine(paths[x].points2[i].position, paths[x].points2[j].position);
            }
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
    
    }
}
