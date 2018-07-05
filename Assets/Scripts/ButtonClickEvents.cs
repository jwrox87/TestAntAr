using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClickEvents : MonoBehaviour
{
    LoadScreenMethods loadScreen;
    private void Awake()
    {
        loadScreen = LoadScreenMethods.Instance;
    }

    public void GoToAntScene()
    {
        loadScreen.StartCoroutine(loadScreen.LoadSceneInOrder("ants_scene_AR_2"));
        //loadScreen.StartCoroutine(loadScreen.LoadActivityIcon("ants_scene_AR_2"));
    }

    public void GoToSpeechScene()
    { 
        loadScreen.StartCoroutine(loadScreen.LoadSceneInOrder("speech_scene"));
        //loadScreen.StartCoroutine(loadScreen.LoadActivityIcon("speech_scene"));
    }

    public void GoToHallucinationScene()
    {
        loadScreen.StartCoroutine(loadScreen.LoadSceneInOrder("illusion_scene"));
        //loadScreen.StartCoroutine(loadScreen.LoadActivityIcon("illusion_scene"));
    }

    public void GoToMenuScene()
    {
        loadScreen.StartCoroutine(loadScreen.LoadSceneInOrder("Menu"));
        //loadScreen.StartCoroutine(loadScreen.LoadActivityIcon("Menu"));
    }

    public void Quit()
    {
        Application.Quit();
    }

}
