using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum MenuPanelState : byte
{
    Middle, 
    Left,
    Right
}

struct StateAdvisor
{
    public MenuPanelState tempb4_b4_state, tempb4_aft_state, tempb4_state;
    public MenuPanelState tempaft_b4_state, tempaft_aft_state, tempaft_state;
}

public class MenuPanelHandler : MonoBehaviour
{
    public UnityEngine.UI.Text debugtext;
    public List<MenuPanel> menuPanels;

    void Init_Menu()
    {
        for (int i = 0; i < menuPanels.Count; i++)
        {
            menuPanels[i].LocalPos = menuPanels[i].transform.localPosition;
            menuPanels[i].state = MenuPanelState.Middle;

            if (i - 1 < 0)
            {
                menuPanels[i].beforePanel = menuPanels[menuPanels.Count - 1].gameObject;
                menuPanels[i].state = MenuPanelState.Left;
            }
            else
                menuPanels[i].beforePanel = menuPanels[i - 1].gameObject;

            if (i + 1 > menuPanels.Count - 1)
            {
                menuPanels[i].afterPanel = menuPanels[0].gameObject;
                menuPanels[i].state = MenuPanelState.Right;
            }
            else
                menuPanels[i].afterPanel = menuPanels[i + 1].gameObject;
        }

        foreach (MenuPanel mp in menuPanels)
            mp.InitStates();


        foreach (MenuPanel mp in menuPanels)
            mp.CopyStates();
    }
    
    // Use this for initialization
    void Start ()
    {
        Init_Menu();
    }

    public void MovePanelsForward(float amount)
    {
        foreach (MenuPanel mp in menuPanels)
            mp.CopyStates();

        foreach (MenuPanel mp in menuPanels)
        {
            if (!mp.IsScrolling)
                mp.StartCoroutine(mp.MoveForward(amount));
        }
    }

    public void MovePanelsBack(float amount)
    {
        foreach (MenuPanel mp in menuPanels)
            mp.CopyStates();

        foreach (MenuPanel mp in menuPanels)
        {
            if (!mp.IsScrolling)
                mp.StartCoroutine(mp.MoveBack(amount));
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (InputHandler.isSwipe() == InputHandler.SwipeDirection.left)
        {
            if (InputHandler.swipeAmount > 100)
                MovePanelsBack(InputHandler.swipeAmount);
        }
        else if (InputHandler.isSwipe() == InputHandler.SwipeDirection.right)
        {
            if (InputHandler.swipeAmount > 100)
                MovePanelsForward(InputHandler.swipeAmount);
        }

    }
}
