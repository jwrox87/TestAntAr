using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreenMethods : MonoBehaviour
{
    #region SINGLETON PATTERN

    public static LoadScreenMethods _instance;
    public static LoadScreenMethods Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LoadScreenMethods>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("LoadScreen");
                    _instance = container.AddComponent<LoadScreenMethods>();
                }
            }

            return _instance;
        }
    }

    #endregion

    GameObject loadingbar;
    private float _loadingProgress;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void OnLevelWasLoaded(int level)
    {
        loadingbar = GameObject.Find("Loadingbar");
    }

    public IEnumerator LoadAsyncIcon(string scenename)
    {
        AsyncOperation async_load = SceneManager.LoadSceneAsync("loading_scene");

        while (!async_load.isDone)
        {
            yield return null;
        }

        SceneManager.LoadScene(scenename);
    }

    public IEnumerator LoadActivityIcon(string scenename)
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

    void LoadingBarUpdate(Transform bar, float load_progress)
    {
        float x_scale = ExtensionMethods<float>.Map(0, 1f, 0, 3.5f, load_progress);

        bar.transform.localScale = new Vector2(x_scale, bar.transform.localScale.y);
    }

    public IEnumerator LoadAsyncConnect(string scenename)
    {
        AsyncOperation async_load = SceneManager.LoadSceneAsync(scenename);
        async_load.allowSceneActivation = false;

        while (!async_load.isDone)
        {
            _loadingProgress = Mathf.Clamp01(async_load.progress / 0.9f);
            if (loadingbar)
            {
                LoadingBarUpdate(loadingbar.transform, _loadingProgress);
            }

            if (async_load.progress >= 0.9f && _loadingProgress >= 1)
                async_load.allowSceneActivation = true;

            yield return null;
        }
    }

    public IEnumerator LoadSceneInOrder(string scenename)
    {
        yield return SceneManager.LoadSceneAsync("loading_scene");
        yield return StartCoroutine(LoadAsyncConnect(scenename));
    }

}
