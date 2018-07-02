using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class AudioManager
{
    static AudioClip[] audioClips;

    public static void SetAudioClip(AudioClip[] audioclips)
    {
        audioClips = audioclips;
    }

    public static AudioClip GetAudioClip(string s)
    {
        foreach (AudioClip ac in audioClips)
        {
            if (ac.name == s)
                return ac;
        }

        return null;
    }

    public static void PlayAudio(AudioSource audioSource, string audioName)
    {
        if (!audioSource)
            return;

        audioSource.clip = GetAudioClip(audioName);

        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}

public class SpeechBubbleObj : MonoBehaviour{

    //Components
    TextMeshPro tmPro;
    AudioSource audioSource;

    //Audio Assistance
    public AudioClip[] audioClips;
    public AudioSource GetAudioSource() { return audioSource; }

    //Attributes
    public bool IsAppearing{ get; set; }
    public bool IsFading { get; set; }

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

    //Coroutines for actions
    public IEnumerator Fade()
    {
        while (GetAlphaValue() > 0 && !IsAppearing)
        {
            IsFading = true;

            SetAlphaValue(GetAlphaValue() - (Time.deltaTime * 0.01f));
            yield return new WaitForSeconds(0.1f);
        }

        IsFading = false;
    }

    public IEnumerator Appear()
    {
        while (GetAlphaValue() <= 1f && !IsFading)
        {
            IsAppearing = true;

            SetAlphaValue(GetAlphaValue() + (Time.deltaTime * 0.01f));
            yield return new WaitForSeconds(0.1f);
        }

        IsAppearing = false;
    }

    public IEnumerator MoveTo(Vector3 destination, float modifier)
    {
        while (Vector3.Distance(transform.position, destination) > 1f)
        {
            transform.position = Vector3.Lerp(transform.position, destination,
                Time.deltaTime * modifier);

            yield return null;
        }
    }


    // Use this for initialization
    void Awake ()
    {
        tmPro = GetComponentInChildren<TextMeshPro>();
        audioSource = GetComponentInChildren<AudioSource>();

        AudioManager.SetAudioClip(audioClips);
    }

}
