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

    string currText;

    public string GetCurrText
    {
        get { return currText; }
    }

    // Use this for initialization
    void Start()
    {
        TextAsset temp = Resources.Load("SpeechBubble") as TextAsset;
        container = SpeechBubbleContainer.LoadFromText(temp.text);

        currText = container.Access(0).text;

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
       speechBubble.GetComponent<SpeechBubbleObj>().ChangeText(currText);

        StartCoroutine(SpeechBubbleLogic(speechBubble.GetComponent<SpeechBubbleObj>()));
       
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


    public IEnumerator SpeechBubbleLogic(SpeechBubbleObj speechbubble)
    {
       
        const float MinAlpha = 0f;
        const float MaxAlpha = 1f;

        float changeDelay = 0.5f;

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
            }

            if (speechbubble.IsFading)
                StartCoroutine(speechbubble.Fade());
            else
            {
                speechbubble.IsAppearing = true;
                speechbubble.IsFading = false;
            }

            //Reset logic
            if (speechbubble.GetAlphaValue() >= MaxAlpha)
            {
                yield return new WaitForSeconds(container.SpeechBubbles[containerIndex].delay);
                speechbubble.IsAppearing = false;
            }

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


    // Update is called once per frame
    void Update () {


    }
}
