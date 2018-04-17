using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Path
{
    public Transform[] points;
}

public class Pathfinder : MonoBehaviour
{
    [SerializeField]
    public Path[] paths;

	// Use this for initialization
	void Start ()
    {
      
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int x = 0; x < paths.Length; x++)
        {
            for (int i = 0, j = 1; i < paths[x].points.Length - 1; i++, j++)
            {
                Gizmos.DrawLine(paths[x].points[i].position, paths[x].points[j].position);
            }
        }
        
    }
	
	// Update is called once per frame
	void Update ()
    {
    
    }
}
