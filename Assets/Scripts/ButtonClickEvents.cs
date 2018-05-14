using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClickEvents : MonoBehaviour
{
    public void GoToAntScene()
    {
        SceneManager.LoadScene("ants_scene_AR_2");   
    }

    public void GoToSpeechScene()
    {
        SceneManager.LoadScene("speech_scene");
    }

    public void GoToHallucinationScene()
    {
        SceneManager.LoadScene("illusion_scene");
    }

    public void GoToMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
