using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fauna_ThirstyState : AStateBehaviour
{
    [Header("References")]
    [SerializeField] private PathFollower pathFollower;
    [SerializeField] private GameObject waterNode;  // The predefined water node
    [SerializeField] private PlayerDetectionTrigger playerDetection;// Reference to the PathFollower script

    
    [Header("Drink Settings")]
    [SerializeField] private float drinkDuration = 3.0f;
    
    [SerializeField] private float currentDrinkTimer = 0.0f;
    private bool hasArrivedAtWater = false;

    //private bool isMoving = false;

    public override bool InitializeState()
    {
        if (pathFollower == null || waterNode == null || playerDetection == null)
        {
            Debug.LogWarning($"Fauna_PatrollingState on {gameObject.name} is missing references");
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
        
        
        pathFollower.OnPathFinished += HandleArrivalAtWater;
        // Pick current node as start
        //pathFollower.SetEndNode(waterNode);  // Just change the END node!
        pathFollower.GenerateNewPath(waterNode);
        
        pathFollower.StartFollowingPath();

    }

    public override void OnStateUpdate()
    {
        
        if (hasArrivedAtWater)
        {
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
        if (playerDetection.IsPlayerInside)
        {
            return (int)EFaunaState.Reacting;
        }
        if (hasArrivedAtWater && currentDrinkTimer < 0f)
        {
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
