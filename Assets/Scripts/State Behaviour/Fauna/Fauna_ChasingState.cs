
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Fauna_ChasingState : AStateBehaviour
{
    [SerializeField] private Transform playerTransform = null;
    private NavMeshAgent agent = null;
    private LineOfSight monsterLineOfSight = null;

    [SerializeField] private float timeToLooseInterest = 3.0f;
    private float timer = 0.0f;

    public override bool InitializeState()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterLineOfSight = GetComponent<LineOfSight>();

        if (agent == null || playerTransform == null || monsterLineOfSight == null)
            return false;

        return true;
    }

    public override void OnStateStart()
    {
        timer = timeToLooseInterest;
    }

    public override void OnStateUpdate()
    {
        // Can We See The Player?
        if (monsterLineOfSight.HasSeenPlayerThisFrame())
        {
            // Reset Timer
            timer = timeToLooseInterest;

            agent.ResetPath();
            agent.SetDestination(playerTransform.position);
        }
        else
        {
            // TIck The Timer
            timer -= Time.deltaTime;
        }
    }

    public override void OnStateEnd()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }

    public override int StateTransitionCondition()
    {
        if (timer < 0)
            return (int)EFaunaState.Patrolling;

        return (int)EFaunaState.Invalid;
    }
}
