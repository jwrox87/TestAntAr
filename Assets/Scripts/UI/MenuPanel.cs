using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public GameObject beforePanel;
    public GameObject afterPanel;
    public Vector2 LocalPos { get; set; }

    public bool IsScrolling { get; set; }

    MenuPanelState b4_state;
    MenuPanelState aft_state;
    public MenuPanelState state;

    StateAdvisor stateAdvisor;

    public UnityEngine.UI.Image Navi_Image { get; set; }

    public void InitStates()
    {
        b4_state = beforePanel.GetComponent<MenuPanel>().state;
        aft_state = afterPanel.GetComponent<MenuPanel>().state;
    }

    public IEnumerator MoveForward(float amount)
    {
        MenuPanel aft_panel = afterPanel.GetComponent<MenuPanel>();
        Vector3 target = aft_panel.LocalPos;

        IsScrolling = true;

        var i = 0.0f;
        var rate = 0.9f;
        while (i < 0.6f)
        {
            i += Time.deltaTime * rate;
            
            if (state != MenuPanelState.Right)
                transform.localPosition = Vector3.Lerp(transform.localPosition, target, i);

            yield return null;
        }
       
        ChangeState_AFT();

        transform.localPosition = target;
        LocalPos = transform.localPosition;

        IsScrolling = false;

    }

    public IEnumerator MoveBack(float amount)
    {
        MenuPanel b4_panel = beforePanel.GetComponent<MenuPanel>();
        Vector3 target = b4_panel.LocalPos;

        IsScrolling = true;

        var i = 0.0f;
        var rate = 0.9f;
        while (i < 0.6f)
        {
            i += Time.deltaTime * rate;

            if (state != MenuPanelState.Left)
                transform.localPosition = Vector3.Lerp(transform.localPosition, target, i);

            yield return null;
        }

        ChangeState_B4();

        transform.localPosition = target;
        LocalPos = transform.localPosition;

        IsScrolling = false;
    }

    public void CopyStates()
    {
        MenuPanel b4_panel = beforePanel.GetComponent<MenuPanel>();
        stateAdvisor.tempb4_aft_state = b4_panel.aft_state;
        stateAdvisor.tempb4_b4_state = b4_panel.b4_state;
        stateAdvisor.tempb4_state = b4_panel.state;

        MenuPanel aft_panel = afterPanel.GetComponent<MenuPanel>();
        stateAdvisor.tempaft_aft_state = aft_panel.aft_state;
        stateAdvisor.tempaft_b4_state = aft_panel.b4_state;
        stateAdvisor.tempaft_state = aft_panel.state;
    }

    public void ChangeState_B4()
    {
        state = stateAdvisor.tempb4_state;
        b4_state = stateAdvisor.tempb4_b4_state;
        aft_state = stateAdvisor.tempb4_aft_state;
    }

    public void ChangeState_AFT()
    {
        state = stateAdvisor.tempaft_state;
        b4_state = stateAdvisor.tempaft_b4_state;
        aft_state = stateAdvisor.tempaft_aft_state;
    }



}
