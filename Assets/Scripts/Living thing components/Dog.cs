using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : LivingObj, IAnimationHandler
{
    public IEnumerator enumerator;
    TimeAllocaterClass time_allocator;

    Dog()
    {
        time_allocator = new TimeAllocaterClass(6);
    }

    public void UpdateAnimationState()
    {
        switch (CurrentState)
        {
            case State.idle:
                GetAnimatorComponent().CrossFade("Idle", 0);
                break;

            case State.move:

                Vector2 time_parameters = time_allocator.TimeAllocation(4, 8);

                if (!GetAnimatorComponent().GetCurrentAnimatorStateInfo(0).IsName("Run") && time_parameters.y > 6)
                {
                    GetAnimatorComponent().CrossFade("Run", 0f, 0, 0, 0);
                }
                else if (!GetAnimatorComponent().GetCurrentAnimatorStateInfo(0).IsName("Walk") && time_parameters.y <= 6)
                {
                    GetAnimatorComponent().CrossFade("Walk", 0f, 0, 0, 0);
                }
                    
                break;
        }
    }


}
