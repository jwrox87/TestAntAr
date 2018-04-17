using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntManager : MonoBehaviour
{

    public Ant[] ants;

    Pathfinder pathfinder;

	// Use this for initialization
	void Start ()
    {
        pathfinder = Global.Instance.Path_Finder;

        foreach (Ant ant in ants)
        {
            ant.currentPath_id = Mathf.Clamp(ant.currentPath_id, 0, pathfinder.paths.Length - 1);

            ant.Initialize();
            
            ant.CorrectFacing(pathfinder.paths[ant.currentPath_id].points[1].position
                - pathfinder.paths[ant.currentPath_id].points[0].position);

            StartCoroutine(ant.FirstRun());
        }
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    foreach (Ant ant in ants)
        {
            ant.DetectSurface();
            ant.MoveUpdate();
        }
	}
}
