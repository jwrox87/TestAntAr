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

    public Transform[] points;
    public Color color = new Color(1, 1, 1, 1);

    public Type type = Type.Linear;
    public Transform loopPoint;

    Transform[] cachedPoints;

    public void Init()
    {
        cachedPoints = points;
    }

    public void Loop()
    {
        //Switch start and end arrays to loop
        points[0] = points[points.Length - 1];
        points[1] = loopPoint;
    }

    public void Skip(int skipIndex)
    {
        Transform skipPoint = points[skipIndex];  
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
            for (int i = 0, j = 1; i < paths[x].points.Length - 1; i++, j++)
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
            for (int i = 0, j = 1; i < paths[x].points.Length - 1; i++, j++)
            {
                Gizmos.color = paths[x].color;
                Gizmos.DrawLine(paths[x].points[i].position, paths[x].points[j].position);
            }
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
    
    }
}
