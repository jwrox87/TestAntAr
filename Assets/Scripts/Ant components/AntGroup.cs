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

    CPath path;

    //Getter Setters
    public float WalkSpeed
    {
        get { return walkSpeed; }
    }
    public List<Transform> CheckPoints
    {
        get { return checkPoints; }
    }

    public void FirstRun()
    {
        StartCoroutine(MoveIncrement());
    }

    WaitForSeconds move_delay = new WaitForSeconds(0.01f);
    public IEnumerator MoveIncrement()
    {
        while (movePercentage < 1)
        {
            movePercentage += walkSpeed * Time.deltaTime;
            yield return move_delay;
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

    void SetRenderQueue()
    {
        var renders = this.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer render in renders)
        {
            render.material.renderQueue = 2002;
        }
    }

    // Use this for initialization
    public void Initialize()
    {
        Invoke("FirstRun", 1f);

        path = Global.Instance.Path_Finder.paths[path_id];

        path_id = Mathf.Clamp(path_id, 0, Global.Instance.Path_Finder.paths.Length - 1);
        checkPoints = Global.Instance.Path_Finder.paths[path_id].points;

        AntTeam = transform.GetComponentsInChildren<Ant>();

        SetRenderQueue();
    }

	// Update is called once per frame
	void Update ()
    {
        PathUpdate();

        switch (path.type)
        {
            case CPath.Type.Loop:
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
