using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAllocaterClass
{
    private int max_duration = 4;
    private float time_til_change = 0;

    TimeAllocaterClass()
    { }

    public TimeAllocaterClass(int max)
    {
        max_duration = max;
    }

    public Vector2 TimeAllocation(int min, int max)
    {
        if (time_til_change >= max_duration)
        {
            max_duration = Random.Range(min, max);
            time_til_change = 0;
        }
        else
        {
            time_til_change += Time.deltaTime;
        }

        return new Vector2(time_til_change, max_duration);
    }

}
