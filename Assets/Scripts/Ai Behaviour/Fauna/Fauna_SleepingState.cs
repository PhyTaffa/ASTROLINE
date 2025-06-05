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
    
    //event used var
    private bool hasFinishedCurrentPath = false;
    public override bool InitializeState()
    {
        if (pathFollower == null || abode == null)
        {
            Debug.LogWarning($"Fauna_PatrollingState on {gameObject.name} is missing references");
            return false;
        }
        
        if (!base.InitializeState())
        {
            Debug.LogWarning("Fauna Wandering State BASE not initialized");
            return false;
        }
        
        if (!base.InitializeState())
        {
            Debug.LogWarning("Fauna Wandering State BASE not initialized");
            return false;
        }
        
        return true;
    }

    public override void OnStateStart()
    {
        //var initialization
        //sleep
        isSupposedToBeAwake = false;
        
        //events subcription
        pathFollower.OnPathFinished += HandlePathFinished;
        
        //starts moving
        pathFollower.GenerateNewPath(abode);
    }

    public override void OnStateUpdate()
    {
    }

    public override void OnStateEnd()
    {
        pathFollower.StopFollowingPath();
        
        pathFollower.OnPathFinished -= HandlePathFinished;
        

    }

    public override int StateTransitionCondition()
    {
        if (isSupposedToBeAwake && hasFinishedCurrentPath)
        {
            pathFollower.GenerateNewPath();
            hasFinishedCurrentPath = false;
            
            return (int)EFaunaState.Idle;
        }
        
        return (int)EFaunaState.Invalid;
    }
    
    private void HandlePathFinished()
    {
        hasFinishedCurrentPath = true;
    }
}
