using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Ant group class. Does movement for a group of ants
 * 
 * 
 */
public class AntGroup : MonoBehaviour
{
    [Range(0,1)]
    public float movePercentage = 0;
    public int path_id;

    [SerializeField]
    float walkSpeed = 1f;

    List<Transform> checkPoints;

    //Ant relation variables
    Ant[] AntTeam;
    Ant AntLeader;

    Path path;

    //Getter Setters
    public float WalkSpeed
    {
        get { return walkSpeed; }
    }
    public List<Transform> CheckPoints
    {
        get { return checkPoints; }
    }

    public IEnumerator FirstRun()
    {
        yield return new WaitForSeconds(1f);

        StartCoroutine(MoveIncrement());
    }

    public IEnumerator MoveIncrement()
    {
        while (movePercentage < 1)
        {
            movePercentage += walkSpeed * Time.deltaTime;
            yield return new WaitForSeconds(0.03f);
        }
    }

    void Loop()
    {
       path.InitLoop();
       movePercentage = 0;
       ResetAllAnts(0);
    }

    void ResetAllAnts(int resetIndexValue)
    {
        foreach (Ant ant in AntTeam)
        {
            ant.ResetIndex(resetIndexValue);
        }
    }

    void Split()
    {
        float splitVal = Random.value;

        if (splitVal > 0.5f)
        {
            checkPoints = path.points;
            return;
        }
        else
        {
            checkPoints = path.points2;
            return;
        }
    }

    void PathUpdate()
    {
        movePercentage = Mathf.Clamp(movePercentage, 0, 1);
        iTween.PutOnPath(this.gameObject, checkPoints.ToArray(), movePercentage);
    }

    // Use this for initialization
    public void Initialize()
    {
        StartCoroutine(FirstRun());

        path = Global.Instance.Path_Finder.paths[path_id];

        path_id = Mathf.Clamp(path_id, 0, Global.Instance.Path_Finder.paths.Length - 1);
        checkPoints = Global.Instance.Path_Finder.paths[path_id].points;

        AntTeam = transform.GetComponentsInChildren<Ant>();
        AntLeader = AntTeam[0];
    }
	
	// Update is called once per frame
	public void Update ()
    {
        PathUpdate();

        switch (path.type)
        {
            case Path.Type.Loop:
            {
                    if (movePercentage >= 1f)
                    {
                        Loop();
                        Split();
                    }
            }
            break;

        }
	}
}
