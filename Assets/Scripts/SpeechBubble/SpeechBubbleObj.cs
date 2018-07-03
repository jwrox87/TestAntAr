using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBubbleObj : MonoBehaviour{

    //State describing the current speech bubble's movements
    public enum SpeechBubbleStatus
    {
        None,
        Moving,
        Moved,
    }

    //Associated Components
    TextMeshPro tmPro;
    AudioSource audioSource;
    ParticleSystem particle_system;
    Collider col;

    //Audio Assistance
    public AudioSource GetAudioSource() { return audioSource; }

    //Attributes
    public bool IsAppearing{ get; set; }
    public bool IsFading { get; set; }
    public SpeechBubbleStatus Speech_BubbleStatus { get; set; }

    //Helper functions
    /*=====================================================================================*/

    public void ChangeText(string s)
    {
        tmPro.text = s;
    }

    public void SetTextAlphaValue(float f)
    {
        tmPro.color = new Color(tmPro.color.r, tmPro.color.g, tmPro.color.b, f);
    }

    public float GetTextAlphaValue()
    {
        return tmPro.color.a;
    }

    /// <summary>
    /// Set Particle system opacity value
    /// </summary>
    /// <param name="ps"></param>
    /// <param name="target_opacity"></param>
    /// <param name="modifier"></param>
    public void SetParticleTransparency(float target_opacity,float modifier)
    {
        target_opacity = Mathf.Clamp(target_opacity, 0, 1);

        var main = particle_system.main;
        float alpha = main.startColor.color.a - modifier; //modifier *= Time.deltatime
        alpha = Mathf.Clamp(alpha, 0, 1);

        main.startColor = new Color(main.startColor.color.r,
          main.startColor.color.g, main.startColor.color.b, alpha);
    }

    public void SetColliderStatus(bool b)
    {
        col.enabled = b;
    }

    //Coroutines for movement or visual
    /*=====================================================================================*/
    /*=====================================================================================*/
    WaitForSeconds waitTime = new WaitForSeconds(0.1f);

    //Coroutines for actions
    public IEnumerator Fade()
    {
        SetColliderStatus(false);

        const float speed = 0.01f;
        while (GetTextAlphaValue() > 0 && !IsAppearing)
        {
            IsFading = true;

            float modifier = Time.deltaTime * speed;
            SetParticleTransparency(0f, modifier);
            SetTextAlphaValue(GetTextAlphaValue() - modifier);

            yield return waitTime;
        }

        IsFading = false;
    }

    public IEnumerator Appear()
    {
        const float speed = 0.01f;
        while (GetTextAlphaValue() <= 1f && !IsFading)
        {
            IsAppearing = true;

            float modifier = Time.deltaTime * speed;
            SetParticleTransparency(0f, -modifier);
            SetTextAlphaValue(GetTextAlphaValue() + modifier);

            yield return waitTime;
        }

        IsAppearing = false;
        SetColliderStatus(true);
    }

    public IEnumerator MoveTo(Vector3 screenpos_destination, float modifier)
    {
        SetColliderStatus(false);
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        while (Vector2.Distance(screenPos, screenpos_destination) > 1f)
        {
            Speech_BubbleStatus = SpeechBubbleStatus.Moving;

            //transform.position = Vector3.Lerp(transform.position, destination,
            //    Time.deltaTime * modifier);
            
            screenPos = Vector2.Lerp(screenPos, screenpos_destination, Time.deltaTime * modifier);
            transform.position = Camera.main.ScreenToWorldPoint(screenPos);

            SetParticleTransparency(0f, .4f * Time.deltaTime);

            yield return null;
        }

        Speech_BubbleStatus = SpeechBubbleStatus.Moved;
    }

    /*=====================================================================================*/
    /*=====================================================================================*/
    /*=====================================================================================*/

    // Use this for initialization
    void Awake ()
    {
        tmPro = GetComponentInChildren<TextMeshPro>();
        audioSource = GetComponentInChildren<AudioSource>();
        particle_system = GetComponentInChildren<ParticleSystem>();
        col = GetComponent<Collider>();
    }

}
