using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods<T>
{
    public static float Randomize(float size)
    {
        return Random.Range(0, size);
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
}
