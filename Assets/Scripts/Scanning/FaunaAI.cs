using UnityEngine;
using UnityEngine.AI;  // Required for AI Navigation

public class FaunaAI : MonoBehaviour
{
    public enum AIState { Idle, Wandering, Fleeing, Observing }
    private AIState currentState = AIState.Wandering;

    public Transform player;
    public float detectionRange = 5f;
    public float fleeSpeed = 6f;
    public float wanderRadius = 10f;
    private NavMeshAgent agent;

    private float idleTime = 2f; // Time to stay idle before moving again
    private float wanderTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Find the player automatically
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        wanderTimer = idleTime;
        Wander();
    }


    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            Flee();  // Run away if the player is too close
        }
        else if (currentState == AIState.Wandering)
        {
            Wander();  // Roam around naturally
        }

        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0 && currentState == AIState.Wandering)
        {
            Wander();
        }
    }

    void Wander()
    {
        currentState = AIState.Wandering;
        Vector3 newPosition = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(newPosition);
        wanderTimer = idleTime + Random.Range(1f, 4f); // Randomize movement times
    }

    void Flee()
    {
        currentState = AIState.Fleeing;
        Vector3 fleeDirection = transform.position - player.position;
        Vector3 fleePosition = transform.position + fleeDirection.normalized * fleeSpeed;
        agent.SetDestination(fleePosition);
    }

    public void Observe() // If the player scans it, the fauna stops moving
    {
        currentState = AIState.Observing;
        agent.isStopped = true;
    }

    public void ResumeMovement()
    {
        agent.isStopped = false;
        Wander();
    }

    // Helper function to find a random point on the NavMesh
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layerMask)
    {
        Vector3 randDirection = Random.insideUnitSphere * distance;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, distance, layerMask);
        return navHit.position;
    }
}
