 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fauna_WanderingingState : AStateBehaviour
{
    [Header("Pathfinding")]
    [SerializeField] private PathFollower pathFollower;  // Reference to the PathFollower script

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float threshold = 10f;

    private List<Node> path;
    private int currentIndex = 0;
    private bool isMoving = false;
    
    private LineOfSight monsterLineOfSight = null;

    private float timer = 0;
    private Rigidbody rb;

    public override bool InitializeState()
    {
        monsterLineOfSight = GetComponent<LineOfSight>();
        rb = GetComponent<Rigidbody>();
        
        if (pathFollower == null || monsterLineOfSight == null)
        {
            Debug.LogWarning($"Fauna_PatrollingState on {gameObject.name} is missing references!");
            return false;
        }
        return true;
    }

    public override void OnStateEnd()
    {
        pathFollower.StopFollowingPath();
    }

    public override void OnStateStart()
    {
        pathFollower.StartFollowingPath();
    }

    public override void OnStateUpdate()
    {
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
        //         if (agent.remainingDistance <= agent.stoppingDistance)
        //         {
        //             return (int)EMonsterState.Idle;
        //         }
        if (timer < 0)
        {
            return (int)(EFaunaState.Idle);
        }

        if (monsterLineOfSight.HasSeenPlayerThisFrame())
        {
            return (int)EFaunaState.Idle;
        }

        return (int)EFaunaState.Invalid;
    }
}
