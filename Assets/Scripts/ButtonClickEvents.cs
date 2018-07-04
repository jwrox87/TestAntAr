using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class ButtonClickEvents : MonoBehaviour
{
    IEnumerator LoadAsyncIcon(string scenename)
    {
        AsyncOperation async_load = SceneManager.LoadSceneAsync("loading_scene");

        while (!async_load.isDone)
        {
            yield return null;
        }

        SceneManager.LoadScene(scenename);
    }

    IEnumerator LoadActivityIcon(string scenename)
    {

#if UNITY_IPHONE
        Handheld.SetActivityIndicatorStyle(iOS.ActivityIndicatorStyle.Gray);
#elif UNITY_ANDROID
        Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Large);
#endif

        Handheld.StartActivityIndicator();

        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scenename);
    }

    IEnumerator LoadAsyncConnect(string scenename)
    {
        AsyncOperation async_load = SceneManager.LoadSceneAsync(scenename);
        async_load.allowSceneActivation = false;

        while (!async_load.isDone)
        {
            if (async_load.progress >= 0.9f)
                async_load.allowSceneActivation = true;

            yield return null;
        }
    }

    IEnumerator LoadSceneInOrder(string scenename)
    {
        yield return SceneManager.LoadSceneAsync("loading_scene");
        yield return StartCoroutine(LoadAsyncConnect(scenename));
    }

    public void GoToAntScene()
    {
        //StartCoroutine(LoadAsyncConnect("ants_scene_AR_2"));
        //StartCoroutine(LoadSceneInOrder("ants_scene_AR_2"));
        StartCoroutine(LoadActivityIcon("ants_scene_AR_2"));
    }

    public void GoToSpeechScene()
    {
        //StartCoroutine(LoadAsyncConnect("speech_scene"));     
        //StartCoroutine(LoadSceneInOrder("speech_scene"));
        StartCoroutine(LoadActivityIcon("speech_scene"));
    }

    public void GoToHallucinationScene()
    {
        //SceneManager.LoadScene("illusion_scene");
        // StartCoroutine(LoadAsyncConnect("illusion_scene"));
        //StartCoroutine(LoadSceneInOrder("illusion_scene"));
        StartCoroutine( LoadActivityIcon("illusion_scene"));
    }

    public void GoToMenuScene()
    {
        //SceneManager.LoadScene("Menu");
        // StartCoroutine(LoadAsyncConnect("Menu"));
        //StartCoroutine(LoadSceneInOrder("Menu"));
        StartCoroutine(LoadActivityIcon("Menu"));
    }

    public void Quit()
    {
        Application.Quit();
    }

}
