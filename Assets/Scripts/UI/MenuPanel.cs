using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public int counter;

    public GameObject beforePanel;
    public GameObject afterPanel;
    public Vector2 localpos;

    public void Move_ToForward(float amount)
    {
        Vector3 target = afterPanel.GetComponent<MenuPanel>().localpos;

        Vector2 vec = transform.localPosition;
        vec.x += amount * Time.deltaTime;

        vec.x = Mathf.Clamp(vec.x, vec.x, target.x);

        transform.localPosition = vec;
    }

    public void Move_ToBack(float amount)
    {
        Vector3 target = beforePanel.GetComponent<MenuPanel>().localpos;

        Vector2 vec = transform.localPosition;
        vec.x -= amount * Time.deltaTime;

        vec.x = Mathf.Clamp(vec.x, target.x, vec.x);

        transform.localPosition = vec;
    }


    public IEnumerator MoveForward(float amount)
    {
        Vector3 target = afterPanel.GetComponent<MenuPanel>().localpos;

        var i = 0.0f;
        var rate = 1.0f;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, i * 10f);
            yield return null;
        }

        localpos = transform.localPosition;
    }

    public IEnumerator MoveBack(float amount)
    {
        Vector3 target = beforePanel.GetComponent<MenuPanel>().localpos;

        var i = 0.0f;
        var rate = 1.0f;
        while (i < 1f)
        {
            i += Time.deltaTime * rate;
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, i * 10f);
            yield return null;
        }

        localpos = transform.localPosition;
    }
}
