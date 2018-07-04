using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods<T>
{
    public static float Randomize(float min, float max)
    {
        return Random.Range(min, max);
    }

    public static float Randomize(float size)
    {
        return Random.Range(0, size);
    }

    public static float RandomUntilNoRepeat(int compare,int Count)
    {
        float first_val = Randomize(Count);

        while (first_val == compare)
        {
            first_val = Randomize(Count);
        }

        return first_val;
    }

    public static void RandomizeList(ref List<T> randomList)
    {
        for (int i = 0; i < randomList.Count; i++)
        {
            T temp = randomList[i];
            int randomIndex = Random.Range(i, randomList.Count);
            randomList[i] = randomList[randomIndex];
            randomList[randomIndex] = temp;
        }
    }

    public static T FindObj(string name)
    {
        GameObject obj = GameObject.Find(name);

        if (obj && obj.GetComponent<T>() != null)
            return obj.GetComponent<T>();
        
        return default(T);
    }

    public static T FindObjWithComponent()
    {      
        GameObject obj = GameObject.FindObjectOfType(typeof(T)) as GameObject;

        if (obj && obj.GetComponent<T>() != null)
            return obj.GetComponent<T>();

        return default(T);
    }

    public static float Map(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

}
