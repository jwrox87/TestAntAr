using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class UICanvasManager : MonoBehaviour
{
    public UIBehaviour[] list_of_ui;

    public void Toggle_All_UI(bool b)
    { 
        foreach (UIBehaviour ui in list_of_ui)
        {
            if (ui)
            ui.gameObject.SetActive(b);
        }       
    }

    public void Toggle_UI_ByType(System.Type type, bool b)
    {
        foreach (UIBehaviour ui in list_of_ui)
        {
            if (ui && ui.GetType() == type)
            {
                ui.gameObject.SetActive(b);
            }
        }
    }


    public void Toggle_UI_ByName(string name, bool b)
    {
        foreach (UIBehaviour ui in list_of_ui)
        {
            if (ui && ui.name == name)
            {
                ui.gameObject.SetActive(b);
            }
        }

    }

	
}
