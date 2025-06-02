 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SLUG_WanderingingState : AStateBehaviour
{
    [Header("References")]
    [SerializeField] private PathFollower pathFollower;
    [SerializeField] private PlayerDetectionTrigger playerDetection;// Reference to the PathFollower script

    [Header("Wander Settings")]
    [SerializeField] private float rotationSpeed = 40f;
    [SerializeField] private float threshold = 10f;

    [Header("Thirst Settings")]
    [SerializeField] private float thirstTimer = 20f;
    
    [Header("Sleep Settings")]
    [SerializeField] private bool isSupposedToSleep = false;
    
    [Header("Idle Settings")]
    [SerializeField] private float timetoGoIdle = 15f;
    
    [Header("Reaction Settings")]
    [SerializeField] private float reactionTimer = 4f;
    
    
    private List<Node> path;
    private int currentIndex = 0;
    private bool isMoving = false;
    
    //private LineOfSight monsterLineOfSight = null;

    [Header("Debug info")]
    [SerializeField] private float currThirstTimer = 0f;
    [SerializeField] private float currIdleTimer = 0f;
    [SerializeField] private float currentReactionTime = 0f;
    [SerializeField] private float currIdleTime = 0f;
    
    private bool hasFinishedCurrentPath = false;

    private Rigidbody rb;

    public override bool InitializeState()
    {
        //monsterLineOfSight = GetComponent<LineOfSight>();
        rb = GetComponent<Rigidbody>();
        
        //other trigger goddddamn
        // if(playerDetection == null)
        //     playerDetection = GetComponent<PlayerDetectionTrigger>();
        //
        if (pathFollower == null || playerDetection == null)
        {
            Debug.LogWarning($"Fauna_PatrollingState on {gameObject.name} is missing references!");
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
        //all of the timer have to be randomized a little, +-30%
        //thirsty
        currThirstTimer = thirstTimer;
        
        //sleep
        isSupposedToSleep = false;
        
        //reaction
        currentReactionTime = reactionTimer;
        
        //idle
        currIdleTimer = timetoGoIdle;
        
        
        //wander
        hasFinishedCurrentPath = false;  // Reset when we enter this state
        pathFollower.autoLoopPaths = true;

        pathFollower.OnPathFinished += HandlePathFinished;
        //pathFollower.GenerateNewPath();
        pathFollower.StartFollowingPath();
    }

    public override void OnStateUpdate()
    {
        //decrease thirst
        currThirstTimer -= Time.deltaTime;
        
        //decrease Idle timer
        currIdleTimer -= Time.deltaTime;
        

        //simulates the reacttion
        if (playerDetection.IsPlayerInside)
        {
            //continues to set the value of a bool to false, a bit haram
            pathFollower.StopFollowingPath();
            
            currentReactionTime = reactionTimer;
        }
        else
        {
            //decreases the timer while player is outside
            currentReactionTime -= Time.deltaTime;

            //once timer runs out, resume following the path
            if (currentReactionTime <= 0)
            {
                Debug.Log($"Lost player contact, continuing to move");
                currentReactionTime = reactionTimer;
                pathFollower.ContinueFollowingPath();
            }
        }
    }

    public override int StateTransitionCondition()
    {
        if (hasFinishedCurrentPath)
        {
            if (currThirstTimer < 0f)
            {
                return (int)EFaunaState.Thirsty;
            }

            if (isSupposedToSleep)
            {
                return (int)EFaunaState.Sleepy;
            }

            if (currIdleTimer < timetoGoIdle)
            {
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
