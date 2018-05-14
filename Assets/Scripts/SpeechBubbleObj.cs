using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBubbleObj : MonoBehaviour {

    TextMeshPro tmPro;

    bool isAppearing = true;
    bool isFading = false;

    public bool IsAppearing
    {
        get { return isAppearing; }
        set { isAppearing = value; }
    }
    public bool IsFading
    {
        get { return isFading; }
        set { isFading = value; }
    }

    public void ChangeText(string s)
    {
        tmPro.text = s;
    }

    public void SetAlphaValue(float f)
    {
        tmPro.color = new Color(tmPro.color.r, tmPro.color.g, tmPro.color.b, f);
    }

    public float GetAlphaValue()
    {
        return tmPro.color.a;
    }

    public IEnumerator Fade()
    {
        while (GetAlphaValue() > 0 && !isAppearing)
        {
            isFading = true;

            SetAlphaValue(GetAlphaValue() - (Time.deltaTime * 0.01f));
            yield return new WaitForSeconds(0.1f);
        }

        isFading = false;
    }

    public IEnumerator Appear()
    {
        while (GetAlphaValue() <= 1f && !isFading)
        {
            isAppearing = true;

            SetAlphaValue(GetAlphaValue() + (Time.deltaTime * 0.01f));
            yield return new WaitForSeconds(0.1f);
        }

        isAppearing = false;
    }
   
	// Use this for initialization
	void Awake ()
    {
        tmPro = GetComponentInChildren<TextMeshPro>();
    }

}
