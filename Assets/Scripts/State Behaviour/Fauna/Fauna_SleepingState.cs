using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Fauna_SleepingState : AStateBehaviour
{
    [Header("References")]
    [SerializeField] private PathFollower pathFollower;
    [SerializeField] private GameObject abode;  // The predefined water node
    
    [FormerlySerializedAs("isAsleep")]
    [Header("Sleeping Settings")]
    [SerializeField] private bool isSupposedToBeAwake = false;
    public override bool InitializeState()
    {
        if (pathFollower == null || abode == null)
        {
            Debug.LogWarning($"{nameof(Fauna_ThirstyState)} is missing references!");
            return false;
        }
        return true;
    }

    public override void OnStateStart()
    {
        //sleep
        isSupposedToBeAwake = false;
        
        //movement
        pathFollower.GenerateNewPath(abode);
    }

    public override void OnStateUpdate()
    {
        // You could check here if close enough to "fall asleep", but since you said it's handled in transition, nothing here yet.
    }

    public override void OnStateEnd()
    {
        // Optional: reset sleeping status if needed
    }

    public override int StateTransitionCondition()
    {
        if (isSupposedToBeAwake)
        {
            return (int)EFaunaState.Wander;
        }
        
        return (int)EFaunaState.Invalid;
    }
}
