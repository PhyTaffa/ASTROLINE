using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fauna_ThirstyState : AStateBehaviour
{
    [Header("References")]
    [SerializeField] private PathFollower pathFollower;
    [SerializeField] private GameObject waterNode;  // The predefined water node
    [HideInInspector] [SerializeField] private PlayerDetectionTrigger playerDetection;// Reference to the PathFollower script

    
    [Header("Drink Settings")]
    [SerializeField] private float drinkDuration = 3.0f;
    
    [Header("Debug")]
    [SerializeField] private float currentDrinkTimer = 0.0f;
    private bool hasArrivedAtWater = false;

    private bool hasFinishedCurrentPath = false;
    public override bool InitializeState()
    {
        if (pathFollower == null || waterNode == null)
        {
            Debug.LogWarning($"Fauna_PatrollingState on {gameObject.name} is missing references");
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
        //necessary stuff for correct behaviour
        hasArrivedAtWater = false;
        currentDrinkTimer = drinkDuration;
        pathFollower.autoLoopPaths = false;
        hasFinishedCurrentPath = false;
        
        
        pathFollower.OnPathFinished += HandleArrivalAtWater;

        
        pathFollower.GenerateNewPath(waterNode);
        
        pathFollower.StartFollowingPath();
    }

    public override void OnStateUpdate()
    {
        
        if (hasArrivedAtWater)
        {
            FaunaAnimator.SetAnimationState(EFaunaAnimatorState.Thirsty);
            currentDrinkTimer -= Time.deltaTime;
        }
    }

    public override void OnStateEnd()
    {
        pathFollower.StopFollowingPath();
        pathFollower.OnPathFinished -= HandleArrivalAtWater;
        
    }

    public override int StateTransitionCondition()
    {
        if (hasArrivedAtWater && currentDrinkTimer < 0f)
        {
            // if (playerDetection.IsPlayerInside)
            // {
            //     return (int)EFaunaState.Reacting;
            // }
            
            pathFollower.GenerateNewPath();
            hasFinishedCurrentPath = false;
            
            return (int)EFaunaState.Wander;
        }

        return (int)EFaunaState.Invalid;
    }
    
    private void HandleArrivalAtWater()
    {
        hasArrivedAtWater = true;
        //isMoving = false;
        //pathFollower.SetEndNode(waterNode);
        pathFollower.StopFollowingPath();
    }
}
