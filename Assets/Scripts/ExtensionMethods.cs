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

        if (obj)
            return obj.GetComponent<T>();
        
        return default(T);
    }
}
