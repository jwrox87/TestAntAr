using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntGroup : MonoBehaviour
{
    public float movePercentage = 0;

    public int path_id;

    Transform[] checkPoints;

    public Transform [] CheckPoints
    {
        get { return checkPoints; }
    }

    void PathUpdate()
    {
        movePercentage = Mathf.Clamp(movePercentage, 0, 1);
        iTween.PutOnPath(this.gameObject, checkPoints, movePercentage);
    }

    public IEnumerator MoveIncrement()
    {
        while (movePercentage < 1)
        {
            movePercentage += 0.2f * Time.deltaTime;

            yield return new WaitForSeconds(0.03f);
        }
    }

    // Use this for initialization
    void Start () {

       
       // StartCoroutine(MoveIncrement());
	}
	
	// Update is called once per frame
	void Update () {

       // checkPoints = Global.Instance.Path_Finder.paths[path_id].points;
       // PathUpdate();
	}
}
