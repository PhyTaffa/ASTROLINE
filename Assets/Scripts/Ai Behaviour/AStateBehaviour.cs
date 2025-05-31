using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This has to be abstract due to having a base class for states.
// Unity does not serialize anything related to interfaces
[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(FaunaAnimator))]
public abstract class AStateBehaviour : MonoBehaviour
{
    protected FaunaAnimator FaunaAnimator;
    public StateMachine AssociatedStateMachine { get; set; }
    
    public virtual bool InitializeState()
    {
        FaunaAnimator = GetComponent<FaunaAnimator>();
        
        if (FaunaAnimator == null)
            Debug.Log($"animator failed");
        
        return FaunaAnimator != null;
    }
    public abstract void OnStateStart();
    public abstract void OnStateUpdate();
    public abstract void OnStateEnd();
    public abstract int StateTransitionCondition();
}
