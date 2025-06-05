using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(Animator))]
public class FaunaAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private EFaunaAnimatorState currentAnimState = EFaunaAnimatorState.Invalid;

    private string animatorProperty = "AnimState";
    
    public void SetAnimationState(EFaunaAnimatorState newState)
    {
        if (newState == currentAnimState)
            return;

        //avoid repetition
        animator.SetInteger(animatorProperty, (int)newState);
        currentAnimState = newState;
    }

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }
}
