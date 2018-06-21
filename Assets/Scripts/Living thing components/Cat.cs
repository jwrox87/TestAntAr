using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : LivingObj, IAnimationHandler
{
    public IEnumerator enumerator;

    Cat()
    {
        
    }

    public void UpdateAnimationState()
    {
        switch (CurrentState)
        {
            case State.idle:
                GetAnimatorComponent().CrossFade("Idle", 0);
                break;

            case State.move:

                if (!GetAnimatorComponent().GetCurrentAnimatorStateInfo(0).IsName("Walk"))                
                    GetAnimatorComponent().CrossFade("Walk", 0f, 0, 0, 0);
                
                break;
        }
    }

}
