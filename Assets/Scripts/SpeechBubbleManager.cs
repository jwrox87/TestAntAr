using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpeechBubbleManager : MonoBehaviour
{
    int randomPointIndex;
    SpeechBubbleContainer container;
    List<Transform> bubblePoints;

    public GameObject speechBubblePrefab;

    string currText;
    float delay;

    // Use this for initialization
    void Start()
    {
        container = SpeechBubbleContainer.Load(
            System.IO.Path.Combine(Application.dataPath, "Resources/SpeechBubble.xml"));

        delay = container.Access(0).delay;
        currText = container.Access(0).text;

        bubblePoints = new List<Transform>();
        foreach (Transform t in transform)
        {
            bubblePoints.Add(t);
        }

        randomPointIndex = 0;
        //randomPointIndex = (int)ExtensionMethods<float>.Randomize(bubblePoints.Count);

        GameObject speechBubble = CreateSpeechBubble(speechBubblePrefab);
        speechBubble.transform.localEulerAngles = new Vector3(90, 0, 0);
        speechBubble.GetComponent<SpeechBubbleObj>().ChangeText(currText);

        StartCoroutine(Logic(speechBubble.GetComponent<SpeechBubbleObj>()));
    }

    void OnDrawGizmos()
    {    
        foreach (Transform t in transform)
        {
            if (t.gameObject.GetInstanceID() == GetInstanceID())
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawCube(t.position,new Vector3(1f,1f,1f));
        }
    }

    
    GameObject CreateSpeechBubble(GameObject obj)
    {
        return Instantiate(speechBubblePrefab, 
            transform.parent.TransformPoint(bubblePoints[randomPointIndex].localPosition),
            bubblePoints[randomPointIndex].rotation,transform.parent);
    }

    void SpawnSpeechBubble(SpeechBubbleObj bubble)
    { 
        bubble.transform.localPosition = bubblePoints[randomPointIndex].localPosition;
    }

    SpeechBubble GetSpeechBubbleAtIndex(int index)
    {
        return container.Access(index);
    }


    IEnumerator Logic(SpeechBubbleObj speechbubble)
    {
        const float MinAlpha = 0f;
        const float MaxAlpha = 1f;

        float changeDelay = 0.5f;

        while (true)
        {
            if (speechbubble.GetAlphaValue() <= MinAlpha)
            {
                yield return new WaitForSeconds(changeDelay);

                if (randomPointIndex < container.SpeechBubbles.Count - 1)
                    randomPointIndex++;
                else
                    randomPointIndex = 0;

                speechbubble.ChangeText(container.SpeechBubbles[randomPointIndex].text);
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
                yield return new WaitForSeconds(delay);
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
