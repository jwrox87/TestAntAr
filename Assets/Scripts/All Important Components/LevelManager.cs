using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public static class LevelManager
{
    static void LoadVuforiaConfig(Vuforia.CameraDevice.CameraDirection cd)
    {   
        VuforiaConfiguration.Instance.Vuforia.CameraDirection = cd;
    }

    public static string GetCurrentLevel()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static CameraDevice.CameraDirection GetCameraDirection()
    {
        return VuforiaConfiguration.Instance.Vuforia.CameraDirection;
    }

    public static void LevelHandler()
    {
        switch (GetCurrentLevel())
        { 
            case "ants_scene_AR_2":
                VuforiaBehaviour.Instance.enabled = true;
                LoadVuforiaConfig(CameraDevice.CameraDirection.CAMERA_DEFAULT);
                break;

            case "illusion_scene":
                VuforiaBehaviour.Instance.enabled = true;
                LoadVuforiaConfig(CameraDevice.CameraDirection.CAMERA_DEFAULT);
                break;

            case "speech_scene":
                VuforiaBehaviour.Instance.enabled = false;
                LoadVuforiaConfig(CameraDevice.CameraDirection.CAMERA_FRONT);
                break;

            case "Menu":
                VuforiaBehaviour.Instance.enabled = false;
                LoadVuforiaConfig(CameraDevice.CameraDirection.CAMERA_DEFAULT);
                break;

            //case "loading_scene":
            //    VuforiaBehaviour.Instance.enabled = false;
            //    break;

        }
    }
}
