using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *  AntManager class. Does the update for all ants.
 * 
 * 
 */
public class AntManager : MonoBehaviour
{
    Ant[] ants;

    Pathfinder pathfinder;

	// Use this for initialization
	void Start ()
    {
        ants = GameObject.FindObjectsOfType<Ant>();

        pathfinder = Global.Instance.Path_Finder; 

        foreach (Ant ant in ants)
        {
            ant.CurrentPathId = Mathf.Clamp(ant.CurrentPathId, 0, pathfinder.paths.Length - 1);

            ant.Initialize();
            
            ant.CorrectFacing(pathfinder.paths[ant.CurrentPathId].points[1].position
                - pathfinder.paths[ant.CurrentPathId].points[0].position);

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
