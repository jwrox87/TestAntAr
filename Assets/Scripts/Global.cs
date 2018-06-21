﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

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
    SpeechBubbleManager speechbubblemanager;
    DebugInfo debuginfo;
    UICanvasManager uICanvasManager;

    public Pathfinder Path_Finder
    {
        get { return pathfinder; }
    }
    public AntManager Ant_Manager
    {
        get { return antmanager; }
    }

    public SpeechBubbleManager SpeechBubble_Manager
    {
        get { return speechbubblemanager; }
    }

    public DebugInfo DebugInfo
    {
        get { return debuginfo; }
    }

    public UICanvasManager UICanvasManager
    {
        get { return uICanvasManager; }
    }

    void Init()
    {
        /*
        if (GameObject.Find("PathFinder"))
            pathfinder = GameObject.Find("PathFinder").GetComponent<Pathfinder>();

        if (GameObject.Find("AntManager"))
            antmanager = GameObject.Find("AntManager").GetComponent<AntManager>();

        if (GameObject.Find("Speech Bubble Points"))
            speechbubblemanager = GameObject.Find("Speech Bubble Points").GetComponent<SpeechBubbleManager>();

        if (GameObject.Find("DebugInfo"))
            debuginfo = GameObject.Find("DebugInfo").GetComponent<DebugInfo>();
            */
    }

    // Use this for initialization
    void Awake ()
    {
        pathfinder = ExtensionMethods<Pathfinder>.FindObj("PathFinder");

        antmanager = ExtensionMethods<AntManager>.FindObj("AntManager");

        speechbubblemanager = ExtensionMethods<SpeechBubbleManager>.FindObj("SpeechBubbleManager");

        debuginfo = ExtensionMethods<DebugInfo>.FindObj("Text");
        //debuginfo = ExtensionMethods<DebugInfo>.FindObjWithComponent();

        uICanvasManager = ExtensionMethods<UICanvasManager>.FindObj("Global");

        //LevelManager.LevelHandler();
    }

    private void Start()
    {
        LevelManager.LevelHandler();    
    }

}
