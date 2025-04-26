 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fauna_WanderingingState : AStateBehaviour
{
    [Header("References")]
    [SerializeField] private PathFollower pathFollower;
    [SerializeField] private PlayerDetectionTrigger playerDetection;// Reference to the PathFollower script

    [Header("Wander Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float threshold = 10f;

    [Header("Thirst Settings")]
    [SerializeField] private float thirstTimer = 20f;
    
    [Header("Sleep Settings")]
    [SerializeField] private bool isSupposedToSleep = false;
    
    private List<Node> path;
    private int currentIndex = 0;
    private bool isMoving = false;
    
    //private LineOfSight monsterLineOfSight = null;

    [Header("Debug")]
    [SerializeField] private float currThirstTimer = 0f;
    
    
    private bool hasFinishedCurrentPath = false;
   
    private float timer = 0;
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
        //thirsty
        currThirstTimer = thirstTimer;
        
        //sleep
        isSupposedToSleep = false;
        
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
        
        // if (isMoving && path != null && currentIndex < path.Count)
        // {
        //     Transform target = path[currentIndex].GetID().transform;
        //     Vector3 targetPos = target.position;
        //     Vector3 direction = (targetPos - transform.position).normalized;
        //
        //     // Rotate toward target
        //     if (direction != Vector3.zero)
        //     {
        //         Quaternion targetRotation = Quaternion.LookRotation(direction);
        //         transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100f);
        //     }
        //
        //     // Move toward target
        //     // Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        //     // rb.MovePosition(newPosition);
        //     transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        //
        //     
        //     // Proceed to next node if close enough
        //     if (Vector3.Distance(transform.position, targetPos) < threshold)
        //     {
        //         currentIndex++;
        //         if (currentIndex >= path.Count)
        //         {
        //             isMoving = false;
        //             Invoke(nameof(GenerateNewPath), 1f); // Optional: small delay before new path
        //         }
        //     }
        // }
    }

    public override int StateTransitionCondition()
    {
        if (playerDetection.IsPlayerInside)
        {
            return (int)EFaunaState.Reacting;
        }
        
        if (timer < 0)
        {
            return (int)(EFaunaState.Idle);
        }

        if (hasFinishedCurrentPath)
        {
            if (currThirstTimer < 0f)
            {
                return (int)EFaunaState.Thirsty;
            }
            else if (isSupposedToSleep)
            {
                return (int)EFaunaState.Sleepy;
            }
            else
            {
                pathFollower.GenerateNewPath();
                hasFinishedCurrentPath = false;
            }
        }



        return (int)EFaunaState.Invalid;
    }
    
    private void HandlePathFinished()
    {
        hasFinishedCurrentPath = true;
    }
}
