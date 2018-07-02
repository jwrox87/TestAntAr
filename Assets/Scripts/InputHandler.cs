using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    static RaycastHit hit;

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


    public static bool isMouseClicked(int index)
    {
        return Input.GetMouseButton(index);
    }

}
