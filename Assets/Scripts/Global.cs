using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{

    #region SINGLETON PATTERN

    public static Global _instance;
    public static Global Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Global>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("Global");
                    _instance = container.AddComponent<Global>();
                }
            }

            return _instance;
        }
    }

    #endregion

    Pathfinder pathfinder;
    AntManager antmanager;

    public Pathfinder Path_Finder
    {
        get { return pathfinder; }
    }
    public AntManager Ant_Manager
    {
        get { return antmanager; }
    }

    // Use this for initialization
    void Start ()
    {
        pathfinder = GameObject.Find("PathFinder").GetComponent<Pathfinder>();
        antmanager = GameObject.Find("AntManager").GetComponent<AntManager>();    
	}
	
	// Update is called once per frame
	void Update ()
    {
      
	}
}
