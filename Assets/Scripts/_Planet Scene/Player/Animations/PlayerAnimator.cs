using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private EPlayerAnimatorStates currentAnimState = EPlayerAnimatorStates.Invalid;

    private string animatorProperty = "AnimState";
    
    public void SetAnimationState(EPlayerAnimatorStates newState)
    {
        if (newState == currentAnimState)
            return;

        //avoid repetition
        animator.SetInteger(animatorProperty, (int)newState);
        currentAnimState = newState;
    }
}
