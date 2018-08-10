using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class DrawPaths
{
    public static void OnDrawGizmos(Transform[] paths, Color color)
    {
        iTween.DrawPathGizmos(paths, color);
    }
}

public class LivingObjManager : MonoBehaviour
{
    public Cat cat;
    public Dog dog;
    public Boy boy;

    public Button btnDog;
    public Button btnBoy;
    public Button btnCat;

    public Toggle toggleCat;
    public Toggle toggleDog;
    public Toggle toggleBoy;

    public Button btnDog_play;
    public Button btnCat_play;
    public Button btnBoy_play;

    private void OnDrawGizmos()
    {
        DrawPaths.OnDrawGizmos(cat.GetWalkPath(), Color.red);
        DrawPaths.OnDrawGizmos(dog.GetWalkPath(), Color.blue);
        DrawPaths.OnDrawGizmos(boy.GetWalkPath(), Color.white);
    }

    void Init()
    {
        //Update living obj enumerators
        cat.enumerator = MoveIncrement(cat, 0.01f);
        dog.enumerator = MoveIncrement(dog, 0.01f);
        boy.enumerator = MoveIncrement(boy, 0.01f);

        //Checkbox
        ToggleLivingObj(cat, toggleCat);
        ToggleLivingObj(dog, toggleDog);
        ToggleLivingObj(boy, toggleBoy);

        StartCoroutine(cat.enumerator);
        StartCoroutine(dog.enumerator);
        StartCoroutine(boy.enumerator);

        cat.CurrentState = State.move;
        dog.CurrentState = State.move;
        boy.CurrentState = State.move;
    }


    void ResetParameters(LivingObj obj)
    {
        obj.PointOnPath = 0;
        obj.CheckPoint = 0;
        obj.GetAnimatorComponent().SetFloat("walkSpeed", 0);

        obj.gameObject.SetActive(false);
    }

    // Use this for initialization
    void Awake()
    {
        InitLoop(cat);
        InitLoop(dog);
        InitLoop(boy);
    }

    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        ResetParameters(cat);
        ResetParameters(dog);
        ResetParameters(boy);
        StopAllCoroutines();
    }

    void LivingObjSpeed(LivingObj obj, Vector2 distance_factor, float min_speed,float maxspeed = Mathf.Infinity)
    {
        float pts_distance = distance_factor.y;

        float newSpeed
            = pts_distance + (obj.GetWalkSpeed()*Time.deltaTime);

        newSpeed = Mathf.Clamp(newSpeed, min_speed, maxspeed);
        obj.GetAnimatorComponent().SetFloat("walkSpeed", newSpeed);
    }


    Vector2 DetermineSpeedFactor(LivingObj obj)
    {
        Vector3 currentPos = obj.transform.position;
        Vector3 currentCheckPt = Vector3.zero;

        obj.CheckPoint = Mathf.Clamp(obj.CheckPoint, 0, obj.GetWalkPath().Length-1);
        float distance = Vector3.Magnitude(obj.GetWalkPath()[obj.CheckPoint].position - currentPos);

        if (distance < (0.01f + obj.GetWalkSpeed()))
        {
            obj.CheckPoint++;
            currentCheckPt = obj.transform.localPosition;
        }

        float distance_between_pts
            = Vector3.Magnitude(obj.GetWalkPath()[obj.CheckPoint-1].localPosition - currentCheckPt);

        return new Vector2(distance, distance_between_pts);
    }


    IEnumerator MoveIncrement(LivingObj obj,float delay)
    {
        obj.transform.position = obj.GetWalkPath()[0].position;

        while (obj.PointOnPath < 1)
        {     
            obj.PointOnPath += obj.GetWalkSpeed() * Time.deltaTime;
            yield return new WaitForSeconds(delay);
        }
    }

    void CorrectFacing(LivingObj obj, float reactionSpeed)
    {
        float futurePoint = obj.PointOnPath + 0.01f;

        Vector3 currentPos = iTween.PointOnPath(obj.GetWalkPath(), obj.PointOnPath);
        Vector3 futurePos = iTween.PointOnPath(obj.GetWalkPath(), futurePoint);

        Vector3 dir = (futurePos - currentPos).normalized;

        obj.transform.forward = Vector3.Lerp(obj.transform.forward, dir, Time.deltaTime * reactionSpeed);
    }

    void PathUpdate(LivingObj obj, float reactionSpeed, bool loop)
    {       
        //Object on path
        iTween.PutOnPath(obj.gameObject, obj.GetWalkPath(), obj.PointOnPath);

        //Adjust rotation
        CorrectFacing(obj, reactionSpeed);

        //Loop path if needed
        if (loop)
        UpdateLoop(obj);
    }


    void InitLoop(LivingObj obj)
    {
        GameObject o = new GameObject("Point Loop");
        o.transform.parent = obj.GetWalkPath()[0].transform.parent;

        o.transform.localPosition = obj.GetWalkPath()[0].localPosition;
    }

    void UpdateLoop(LivingObj obj)
    { 
        obj.PointOnPath = Mathf.Clamp(obj.PointOnPath,0,1);

        if (obj.PointOnPath >= 1f)
        {
            obj.PointOnPath = 0;
            obj.CheckPoint = 0;
        }
    }

    public void Toggle_Cat_CheckBox(Toggle toggle)
    {
        ToggleLivingObj(cat, toggle);
    }

    public void Toggle_Dog_CheckBox(Toggle toggle)
    {
        ToggleLivingObj(dog, toggle);
    }

    public void Toggle_Boy_CheckBox(Toggle toggle)
    {
        ToggleLivingObj(boy, toggle);
    }

    void ToggleLivingObj(LivingObj obj, Toggle toggle, IEnumerator enumerator = null)
    {
        if (!toggle.isOn)
            obj.gameObject.SetActive(false);
        else
            obj.gameObject.SetActive(true);
    }

    public void DogButtonLogic()
    {
        ToggleButtonState(btnDog);
        toggleDog.isOn = !toggleDog.isOn;
        btnDog_play.gameObject.SetActive(toggleDog.isOn);
    }

    public void CatButtonLogic()
    {
        ToggleButtonState(btnCat);
        toggleCat.isOn = !toggleCat.isOn;
        btnCat_play.gameObject.SetActive(toggleCat.isOn);
    }

    public void BoyButtonLogic()
    {
        ToggleButtonState(btnBoy);
        toggleBoy.isOn = !toggleBoy.isOn;
        btnBoy_play.gameObject.SetActive(toggleBoy.isOn);
    }

    bool dog_isPlaying = true;
    public void DogPlayButtonLogic()
    {
        ToggleButtonState(btnDog_play);
        dog_isPlaying = !dog_isPlaying;
        SwitchAnimation(dog, dog.enumerator, dog_isPlaying);
    }

    bool cat_isPlaying = true;
    public void CatPlayButtonLogic()
    {
        ToggleButtonState(btnCat_play);
        cat_isPlaying = !cat_isPlaying;
        SwitchAnimation(cat, cat.enumerator, cat_isPlaying);
    }

    bool boy_isPlaying = true;
    public void BoyPlayButtonLogic()
    {
        ToggleButtonState(btnBoy_play);
        boy_isPlaying = !boy_isPlaying;
        SwitchAnimation(boy, boy.enumerator, boy_isPlaying);
    }

    void ToggleButtonState(Button btn)
    {
        Image temp_image = btn.GetComponent<Image>();

        Sprite normalSprite = temp_image.sprite;
        temp_image.sprite = btn.spriteState.pressedSprite;

        SpriteState spriteState = btn.spriteState;

        spriteState.pressedSprite = normalSprite;
        btn.spriteState = spriteState;
    }

    void SwitchAnimation(LivingObj obj, IEnumerator enumerator, bool isPlaying)
    {
        if (isPlaying == false)
        {
            StopCoroutine(enumerator);
            obj.CurrentState = State.idle;
        }
        else
        {
            StartCoroutine(enumerator);
            obj.CurrentState = State.move;
        }
    }


    bool ObjUpdate(LivingObj obj, float reactionSpeed, float min_speed,bool loop)
    {
        PathUpdate(obj, reactionSpeed, loop);

        Vector2 ani_spd = DetermineSpeedFactor(obj);

        if (!obj.isActiveAndEnabled)
            return false;

        LivingObjSpeed(obj, ani_spd, min_speed,1.5f);

        return true;
    }

    // Update is called once per frame
    void Update ()
    {
        if (ObjUpdate(cat, 3f,0.8f, true))
            cat.UpdateAnimationState();

        if (ObjUpdate(dog, 3f,0f, true))
            dog.UpdateAnimationState();

        if (ObjUpdate(boy, 10f,0.8f, true))
            boy.UpdateAnimationState();
    }
}
