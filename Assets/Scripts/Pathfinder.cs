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
    public Color color = new Color(1,1,1,1);

    public Type type = Type.Linear;
    public Transform loopPoint;
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
        for (int x = 0; x < paths.Length; x++)
        {
            iTween.DrawPathGizmos(paths[x].points,paths[x].color);         
        }
        
    }
	
	// Update is called once per frame
	void Update ()
    {
    
    }
}
