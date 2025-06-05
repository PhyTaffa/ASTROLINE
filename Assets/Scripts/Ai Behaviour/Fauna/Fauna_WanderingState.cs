 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[RequireComponent(typeof(PathFollower))]
public class Fauna_WanderingingState : AStateBehaviour
{
    [Header("References")]
    [SerializeField] private PathFollower pathFollower;
    [SerializeField] private PlayerDetectionTrigger playerDetection;// Reference to the PathFollower script
    
    [Header("Thirst Settings")]
    [SerializeField] private float timeToGetThirty = 20f;
    
    [Header("Sleep Settings")]
    [SerializeField] private bool isSupposedToSleep = false;
    
    [Header("Idle Settings")]
    [SerializeField] private float timeToGoIdle = 10f;
    
    [Header("Reaction Settings")]
    [SerializeField] private float reactionTimer = 4f;
    
    
    private List<Node> path;
    private int currentIndex = 0;
    private bool isMoving = false;
    
    //private LineOfSight monsterLineOfSight = null;

    [Header("Debug info")] 
    [SerializeField] private float minRandomTimer = 0.8f;
    [SerializeField] private float maxRandomTimer = 1.2f;
    [SerializeField] private float timeLeftToDrink = 0f;
    [SerializeField] private float timeLeftToIdle = 0f;
    [SerializeField] private float currentReactionTime = 0f;
    
    private bool hasFinishedCurrentPath = false;
   
    private float timer = 0;
    private Rigidbody rb;

    public override bool InitializeState()
    {
        //monsterLineOfSight = GetComponent<LineOfSight>();
        rb = GetComponent<Rigidbody>();
        if(pathFollower == null)
            pathFollower = GetComponent<PathFollower>();
    
        //other trigger goddddamn
        // if(playerDetection == null)
        //     playerDetection = GetComponent<PlayerDetectionTrigger>();
        
        if (pathFollower == null)
        {
            Debug.LogWarning($"Fauna_PatrollingState on {gameObject.name} is missing references!");
            return false;
        }

        // if (playerDetection == null)
        // {
        //     Debug.LogWarning($"Fauna_PatrollingState on {gameObject.name} is missing references!");
        //     return false;
        // }
        
        if (!base.InitializeState())
        {
            Debug.LogWarning("Fauna Wandering State BASE not initialized");
            return false;
        }
        
        
        
        return true;
    }

    public override void OnStateEnd()
    {
        pathFollower.StopFollowingPath();
        pathFollower.OnPathFinished -= HandlePathFinished;
    }

    public override void OnStateStart()
    {
        //Setting parameters, missing a small +- 20% randomizazion for the floats
        //thirsty
        timeLeftToDrink = timeToGetThirty * Random.Range(minRandomTimer, maxRandomTimer);
        
        //sleep
        isSupposedToSleep = false;
        
        //reaction
        currentReactionTime = reactionTimer;
        
        //idle
        timeLeftToIdle = timeToGoIdle * Random.Range(minRandomTimer, maxRandomTimer);;
        
        //wander
        hasFinishedCurrentPath = false;  // Reset when we enter this state
        pathFollower.autoLoopPaths = true;

        pathFollower.OnPathFinished += HandlePathFinished;
        //pathFollower.GenerateNewPath();
        pathFollower.StartFollowingPath();
        
        //animation
        FaunaAnimator.SetAnimationState(EFaunaAnimatorState.Wander);
    }

    public override void OnStateUpdate()
    {
        //decrease thirst & idle
        timeLeftToDrink -= Time.deltaTime;
        timeLeftToIdle -= Time.deltaTime;
        
        //idle could be not a state but inside the thing
        
        
        //simulates the reacttion
        // if (playerDetection.IsPlayerInside)
        // {
        //     //continues to set the value of a bool to false, a bit haram
        //     pathFollower.StopFollowingPath();
        //     
        //     currentReactionTime = reactionTimer;
        // }
        // else
        // {
        //     //decreases the timer while player is outside
        //     currentReactionTime -= Time.deltaTime;
        //
        //     //once timer runs out, resume following the path
        //     if (currentReactionTime <= 0)
        //     {
        //         currentReactionTime = reactionTimer;
        //         pathFollower.ContinueFollowingPath();
        //     }
        // }
    }

    public override int StateTransitionCondition()
    {
        
        // if (timer < 0)
        // {
        //     return (int)(EFaunaState.Idle);
        // }

        if (hasFinishedCurrentPath)
        {
            if (timeLeftToDrink < 0f)
            {
                return (int)EFaunaState.Thirsty;
            }

            if (isSupposedToSleep)
            {
                //return (int)EFaunaState.Sleepy;
            }

            if (timeLeftToIdle < 0f)
            {
                pathFollower.GenerateNewPath();
                hasFinishedCurrentPath = false;
                
                return (int)EFaunaState.Idle;
            }

            
            //necessary to allow repathing before and or after transition
            pathFollower.GenerateNewPath();
            hasFinishedCurrentPath = false;
        }



        return (int)EFaunaState.Invalid;
    }
    
    private void HandlePathFinished()
    {
        hasFinishedCurrentPath = true;
    }
}
