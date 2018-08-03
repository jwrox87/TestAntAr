using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoleculeMsgHandler : MonoBehaviour {

    public enum MsgState
    {
        wrong, correct
    }

    public TextMeshProUGUI msg;
    Image bg;

	// Use this for initialization
	void Start ()
    {
        bg = GetComponent<Image>();
        ShowMsg(false);
    }

    void WriteMsg(string s)
    {
        msg.text = s;
    }

    public void ShowMsg(bool b)
    {
        msg.enabled = b;
        bg.enabled = b;
    }

    public void ShowMsg(MsgState state,bool b)
    {
        if (state == MsgState.correct)
            WriteMsg("Correct Answer");
        else
            WriteMsg("Wrong Answer");

        msg.enabled = b;
        bg.enabled = b;
    }

}
