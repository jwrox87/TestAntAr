using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    public enum SwipeDirection
    {
        none,
        right,
        left,
        top,
        down
    }

    static RaycastHit hit;

    private void Start()
    {
        dragDistance = Screen.height * 15 / 100;
    }

    public static Transform Touch_GetHitObj()
    {
        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                var ray = Camera.main.ScreenPointToRay(t.position);

                if (Physics.Raycast(ray, out hit))
                    return hit.transform;
            }
        }

        return null;
    }

    public static bool isTouched()
    {
        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
                return true;
        }

        return false;
    }

    public static Transform Mouse_GetHitObj()
    {
        Vector3 mousePos = Input.mousePosition;

        var ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray,out hit))
            return hit.transform;

        return null;
    }

    static Vector3 first_press, last_press;
    public static SwipeDirection Mouse_GetSwipeDirection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            first_press = Input.mousePosition;
            last_press = Input.mousePosition;
        }
        //if (Input.mousePosition != first_press && Input.GetMouseButtonDown(0))
        //{
        //    last_press = Input.mousePosition;
        //}
        else if (Input.GetMouseButtonUp(0))
        {
            last_press = Input.mousePosition;
            
            if (Mathf.Abs(last_press.x - first_press.x) > dragDistance ||
                 Mathf.Abs(last_press.y - first_press.y) > dragDistance)
            {
                if (Mathf.Abs(last_press.x - first_press.x) > Mathf.Abs(last_press.y - first_press.y))
                {
                    swipeAmount = Mathf.Abs((last_press - first_press).magnitude);
                   
                    if (last_press.x > first_press.x)
                    {
                        //right swipe
                        return SwipeDirection.right;
                    }
                    else
                    {
                        //left swipe
                        return SwipeDirection.left;
                    }
                }
           
            }

            else
            {
                swipeAmount = 0;
                return SwipeDirection.none;
            }

        }

        
        return SwipeDirection.none;
    }


    public static bool isMouseClicked(int index)
    {
        return Input.GetMouseButton(index);
    }

    public static float swipeAmount { get; set; }

    private static Vector3 lt;
    private static Vector3 ft;
    private static float dragDistance;
    public static SwipeDirection isSwipe()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                ft = touch.position;
                lt = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                lt = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                lt = touch.position;

                if (Mathf.Abs(lt.x - ft.x) > dragDistance || 
                    Mathf.Abs(lt.y - ft.y) > dragDistance)
                {
                    if (Mathf.Abs(lt.x - ft.x) > Mathf.Abs(lt.y - ft.y))
                    {
                        swipeAmount = Mathf.Abs(lt.x - ft.x);

                        if (lt.x > ft.x)
                        {
                            //right swipe
                            return SwipeDirection.right;
                        }
                        else
                        {
                            //left swipe
                            return SwipeDirection.left;
                        }
                    }
                    else
                    {
                        swipeAmount = Mathf.Abs(lt.y - ft.y);

                        if (lt.y > ft.y)
                        {
                            //up swipe
                            return SwipeDirection.top;
                        }
                        else
                        {
                            //down swipe
                            return SwipeDirection.down;
                        }
                    }
                }
                else
                {
                    swipeAmount = 0;
                    return SwipeDirection.none;
                }

            }
        }

        return SwipeDirection.none;
    }

    

}
