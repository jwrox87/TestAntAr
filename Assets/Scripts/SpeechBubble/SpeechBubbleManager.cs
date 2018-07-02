using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class SpeechBubbleManager : MonoBehaviour
{
    int containerIndex;
    int posIndex;
    SpeechBubbleContainer container;
    List<Transform> bubblePoints;

    public Transform speechBubblePoints;
    public GameObject speechBubblePrefab;

    private GameObject speechBubbleObj;
    public GameObject SpeechBubbleObj
    {
        get { return speechBubbleObj; }
        set { speechBubbleObj = value; }
    }

    public Vector2 Face_Rect_Pos { get; set; }

    bool audioPlaying = false;

    // Use this for initialization
    void Start()
    {
        TextAsset temp = Resources.Load("SpeechBubble") as TextAsset;
        container = SpeechBubbleContainer.LoadFromText(temp.text);

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
        speechBubble.GetComponent<SpeechBubbleObj>().ChangeText(container.Access(0).text);

        speechBubbleObj = speechBubble;

        StartCoroutine(ToggleBubbleVisibility(false, 0));
        StartCoroutine(SpeechBubbleUpdate(speechBubble.GetComponent<SpeechBubbleObj>()));   
    }

    void OnDrawGizmos()
    {    
        foreach (Transform t in speechBubblePoints)
        {
            if (t.gameObject.GetInstanceID() == GetInstanceID())
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawCube(t.position,new Vector3(5f,5f,5f));
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

        foreach (Transform t in speechBubbleObj.transform)
            t.gameObject.SetActive(b);
    }

    /// <summary>
    /// Update speech bubble actions
    /// </summary>
    const float MinAlpha = 0f;
    const float MaxAlpha = 1f; 
    const float changeDelay = 0.5f;

    public IEnumerator SpeechBubbleUpdate(SpeechBubbleObj speechbubble)
    {      
        while (true)
        {         
            if (speechbubble.GetAlphaValue() <= MinAlpha)
            {
                yield return new WaitForSeconds(changeDelay);

                if (containerIndex < container.SpeechBubbles.Count - 1)
                    containerIndex++;
                else
                    containerIndex = 0;

                posIndex = (int)ExtensionMethods<float>.Randomize(bubblePoints.Count);
                speechbubble.transform.localPosition 
                    = speechBubblePoints.parent.InverseTransformPoint(bubblePoints[posIndex].position);

                speechbubble.ChangeText(container.SpeechBubbles[containerIndex].text);
                audioPlaying = false;
            }

            if (speechbubble.IsFading)
                StartCoroutine(speechbubble.Fade());
            else
            {
                speechbubble.IsAppearing = true;
                speechbubble.IsFading = false;
            }

            //Reset logic
            /*------------------------------------*/
            /*------------------------------------*/
            if (speechbubble.GetAlphaValue() >= MaxAlpha)
            {
                yield return new WaitForSeconds(container.SpeechBubbles[containerIndex].delay);
                speechbubble.IsAppearing = false;

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
                StartCoroutine(speechbubble.Appear());
            else
            {
                speechbubble.IsAppearing = false;
                speechbubble.IsFading = true;
            }

            yield return null;
        }
    }


    void OnClickMove()
    {
        if (InputHandler.isMouseClicked(0))
        {
            Transform hitObj = InputHandler.Mouse_GetHitObj();

            if (hitObj)
            {
                StopAllCoroutines();

                Vector3 worldPos = Camera.main.
                    ScreenToWorldPoint(Global.Instance.SpeechBubble_Manager.Face_Rect_Pos * 1.5f);

                StartCoroutine(speechBubbleObj.GetComponent<SpeechBubbleObj>().MoveTo(worldPos, 2f));
            }
        }
    }

    private void Update()
    {
        OnClickMove();
    }

}
