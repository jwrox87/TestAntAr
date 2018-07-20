using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public static class AudioManager
{
    static AudioClip[] audioClips;

    public static void SetAudioClips(AudioClip[] audioclips)
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

    public static void StopAudio(AudioSource audioSource)
    {
        if (!audioSource)
            return;

        audioSource.Stop();
    }
}

public class SpeechBubbleManager : MonoBehaviour
{
    int containerIndex;
    int posIndex;
    bool audioPlaying = false;

    SpeechBubbleContainer container;
    List<Transform> bubblePoints;

    public Transform speechBubblePoints;
    public GameObject speechBubblePrefab;

    private SpeechBubbleObj speechBubbleObj;
    public SpeechBubbleObj SpeechBubbleObj {
        get { return speechBubbleObj; }
    }

    public AudioClip[] audioClips;

    public Vector2 Face_Rect_Pos { get; set; }

    // Use this for initialization
    void Start()
    {
        TextAsset temp = Resources.Load("SpeechBubble") as TextAsset;
        container = SpeechBubbleContainer.LoadFromText(temp.text);

        AudioManager.SetAudioClips(audioClips);

        bubblePoints = new List<Transform>();
        foreach (Transform t in speechBubblePoints)
        {
            bubblePoints.Add(t);
        }

        containerIndex = 0;
        posIndex = 0;

        GameObject speechBubble = CreateSpeechBubble(speechBubblePrefab);
        speechBubble.transform.localPosition = new Vector3(0, 0, 0);
        speechBubble.transform.localEulerAngles = new Vector3(90, 0, 0);

        speechBubbleObj = speechBubble.GetComponent<SpeechBubbleObj>();
        speechBubbleObj.ChangeText(container.Access(0).text);

        StartCoroutine(ToggleBubbleVisibility(false, 0));
        StartCoroutine(SpeechBubbleVisualUpdate(speechBubbleObj));
    }

    void OnDrawGizmos()
    {
        foreach (Transform t in speechBubblePoints)
        {
            if (t.gameObject.GetInstanceID() == GetInstanceID())
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawCube(t.position, new Vector3(5f, 5f, 5f));
        }
    }

    public GameObject CreateSpeechBubble(GameObject obj)
    {
        return Instantiate(speechBubblePrefab,
            speechBubblePoints.parent.TransformPoint(bubblePoints[posIndex].localPosition),
            bubblePoints[posIndex].rotation, speechBubblePoints.parent);
    }


    void SpawnSpeechBubble(SpeechBubbleObj bubble)
    {
        bubble.transform.localPosition = bubblePoints[containerIndex].localPosition;
    }

    SpeechBubble GetSpeechBubbleAtIndex(int index)
    {
        return container.Access(index);
    }

    /// <summary>
    /// Speechbubble objects cannot turn themselves on.
    /// </summary>
    /// <param name="b"> Toggle </param>
    /// <param name="delay"> Delay time</param>
    /// <returns></returns>
    public IEnumerator ToggleBubbleVisibility(bool b, float delay)
    {
        yield return new WaitForSeconds(delay);

        Global.Instance.EyeTracker.ToggleEye(b);

        foreach (Transform t in speechBubbleObj.transform)
            t.gameObject.SetActive(b);
    }

    //Helper variables
    const float MinAlpha = 0f;
    const float MaxAlpha = 1f;
    float cached_containerIndex;

    /// <summary>
    /// Update speech bubble actions
    /// </summary>
    public IEnumerator SpeechBubbleVisualUpdate(SpeechBubbleObj speechbubble,
        float changeDelay = 0.5f)
    {
        speechbubble.Speech_BubbleStatus = SpeechBubbleStatus.None;
        WaitForSeconds waitTime = new WaitForSeconds(changeDelay);

        while (true)
        {
            if (speechbubble.GetTextAlphaValue() <= MinAlpha)
            {
                yield return waitTime;

                containerIndex = Random.Range(0, container.SpeechBubbles.Count - 1);

                if (cached_containerIndex != containerIndex)
                    cached_containerIndex = containerIndex;
                else
                    containerIndex = Random.Range(0, container.SpeechBubbles.Count - 1);

                //if (containerIndex < container.SpeechBubbles.Count - 1)
                //    containerIndex++;
                //else
                //    containerIndex = 0;

                posIndex = (int)ExtensionMethods<float>.Randomize(bubblePoints.Count);
                speechbubble.transform.localPosition
                    = speechBubblePoints.parent.InverseTransformPoint(bubblePoints[posIndex].position);

                speechbubble.ChangeText(container.SpeechBubbles[containerIndex].text);
                audioPlaying = false;
            }

            if (speechbubble.IsFading)
                StartCoroutine(speechbubble.Fade(0.05f));
            else
            {
                speechbubble.IsAppearing = true;
                speechbubble.IsFading = false;
            }

            //Reset logic
            /*------------------------------------*/
            /*------------------------------------*/
            if (speechbubble.GetTextAlphaValue() >= MaxAlpha)
            {
                yield return new WaitForSeconds(container.SpeechBubbles[containerIndex].delay);
                speechbubble.IsAppearing = false;
            }
            else if (speechbubble.GetTextAlphaValue() >= 0.4f)
            {
                if (!audioPlaying)
                {
                    AudioManager.PlayAudio(speechbubble.GetAudioSource(),
                        container.SpeechBubbles[containerIndex].identifier);

                    audioPlaying = true;
                }
            }
            /*------------------------------------*/
            /*------------------------------------*/

            if (speechbubble.IsAppearing)
                StartCoroutine(speechbubble.Appear(0.05f));
            else
            {
                speechbubble.IsAppearing = false;
                speechbubble.IsFading = true;
            }

            yield return null;
        }
    }

    delegate void Event_Handle();
    Event_Handle event_handler;

    void Event_Movement()
    {
        StopAllCoroutines();
        AudioManager.StopAudio(speechBubbleObj.GetAudioSource());

        speechBubbleObj.SetParticleTransparency(0, .4f * Time.deltaTime);

        StartCoroutine(speechBubbleObj.MoveTo(Face_Rect_Pos, 2f));
    }

    void Event_Idle()
    {
        speechBubbleObj.SetParticleColor(Color.white, 1f);
        speechBubbleObj.SetTextAlphaValue(0);
        
        StartCoroutine(SpeechBubbleVisualUpdate(speechBubbleObj,1f));
    }

    //Should not be here
    void Event_Update()
    {
        switch (speechBubbleObj.Speech_BubbleStatus)
        {
            case SpeechBubbleStatus.None:

                event_handler = null;

                if (InputHandler.isMouseClicked(0))
                {
                    //Put in hit obj with type of input
                    Transform hitObj = InputHandler.Mouse_GetHitObj();

                    if (hitObj)
                        event_handler = Event_Movement;
                }
                break;


            case SpeechBubbleStatus.Moved:

                event_handler = Event_Idle;
                break;
        }

        if (event_handler != null)
            event_handler();
    }


    private void Update()
    {
        Event_Update();
    }

}
