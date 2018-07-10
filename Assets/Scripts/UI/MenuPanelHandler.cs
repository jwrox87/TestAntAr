using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanelHandler : MonoBehaviour
{
    public List<MenuPanel> menuPanels;


    void Init()
    {
        for (int i = 0; i < menuPanels.Count; i++)
        {
            menuPanels[i].localpos = menuPanels[i].transform.localPosition;
            
            if (i - 1 < 0)
                menuPanels[i].beforePanel = menuPanels[menuPanels.Count-1].gameObject;
            else
                menuPanels[i].beforePanel = menuPanels[i - 1].gameObject;

            if (i + 1 > menuPanels.Count - 1)
                menuPanels[i].afterPanel = menuPanels[0].gameObject;
            else
                menuPanels[i].afterPanel = menuPanels[i + 1].gameObject;
        }
    }

	// Use this for initialization
	void Start ()
    {
        Init();
    }

    void MovePanelsForward(float amount)
    {
        for (int i = 0; i < menuPanels.Count; i++)
        {
             menuPanels[i].StartCoroutine(menuPanels[i].MoveForward(amount));
        }
    }

    void MovePanelsBack(float amount)
    {
        for (int i = 0; i < menuPanels.Count; i++)
        {
            menuPanels[i].StartCoroutine(menuPanels[i].MoveBack(amount));
            // menuPanels[i].Move_ToBack(amount);
        }
    }
	
	// Update is called once per frame
	void Update () {

        //if (InputHandler.isSwipe() == InputHandler.SwipeDirection.left)
        //{
        //    float left_swipe = InputHandler.swipeAmount;

        //    MovePanelsBack(left_swipe);            
        //}

        //if (InputHandler.isSwipe() == InputHandler.SwipeDirection.right)
        //{
        //    float right_swipe = InputHandler.swipeAmount;

        //    MovePanelsForward(right_swipe);
        //}

      
        if (InputHandler.Mouse_GetSwipeAmount() != InputHandler.SwipeDirection.none)
        {
            if (InputHandler.Mouse_GetSwipeAmount() == InputHandler.SwipeDirection.left)
            {
                MovePanelsBack(InputHandler.swipeAmount);
                //print("Left: " +InputHandler.swipeAmount);

            }

            if (InputHandler.Mouse_GetSwipeAmount() == InputHandler.SwipeDirection.right)
            {
                MovePanelsForward(InputHandler.swipeAmount);
                //print("Right: " +InputHandler.swipeAmount);
            }

        }
       

        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    MovePanelsBack();
        //}

        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    MovePanelsForward();
        //}

    }
}
