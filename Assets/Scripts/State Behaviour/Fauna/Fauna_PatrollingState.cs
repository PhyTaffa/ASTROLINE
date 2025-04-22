 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fauna_PatrollingState : AStateBehaviour
{
    [SerializeField] private POIManager poiManager = null;
    private int lastPOIRequested = 0;

    private NavMeshAgent agent = null;
    private LineOfSight monsterLineOfSight = null;

    private float timer = 0;

    public override bool InitializeState()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterLineOfSight = GetComponent<LineOfSight>();

        if (agent == null || monsterLineOfSight == null)
            return false;
        return true;
    }

    public override void OnStateEnd()
    {
        agent.isStopped = true;
    }

    public override void OnStateStart()
    {
        if (!poiManager.IsIndexValid(lastPOIRequested))
        {
            lastPOIRequested = 0;
        }

        Transform poiTransform = poiManager.GetPOIAtIndex(lastPOIRequested);
        if (poiTransform)
        {
            agent.isStopped = false;
            agent.SetDestination(poiTransform.position);

            lastPOIRequested++;
        }
    }

    public override void OnStateUpdate()
    {


        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!poiManager.IsIndexValid(lastPOIRequested))
            {
                lastPOIRequested = 0;
            }

            Transform poiTransform = poiManager.GetPOIAtIndex(lastPOIRequested);
            if (poiTransform)
            {
                agent.isStopped = false;
                agent.SetDestination(poiTransform.position);

                lastPOIRequested++;
            }
        }
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
            return (int)EFaunaState.Chasing;
        }

        return (int)EFaunaState.Invalid;
    }
}
