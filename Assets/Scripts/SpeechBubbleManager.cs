using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpeechBubbleManager : MonoBehaviour
{
    int randomIndex; 
    List<Transform> bubblePoints;

    public GameObject speechBubblePrefab;
    
    // Use this for initialization
    void Start ()
    {
        var container = SpeechBubbleContainer.Load(
            System.IO.Path.Combine(Application.dataPath, "Resources/SpeechBubble.xml"));

        bubblePoints = new List<Transform>();
        foreach (Transform t in transform)
        {
            bubblePoints.Add(t);
        }

        randomIndex = (int)ExtensionMethods<float>.Randomize(bubblePoints.Count);

        GameObject speechBubble = CreateSpeechBubble(speechBubblePrefab);
        speechBubble.transform.localEulerAngles = new Vector3(90, 0, 0);
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
            transform.parent.TransformPoint(bubblePoints[randomIndex].localPosition),
            bubblePoints[randomIndex].rotation,transform.parent);
    }

    void SpawnSpeechBubble(GameObject obj)
    {

    }

    // Update is called once per frame
    void Update () {
 
	}
}
