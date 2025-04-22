using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fauna_IdleState : AStateBehaviour
{
    [SerializeField] private float maxTimer = 1.0f;
    float currentTimer = 0.0f;

    public override bool InitializeState()
    {
        return true;
    }

    public override void OnStateStart()
    {
        currentTimer = maxTimer;
    }

    public override void OnStateUpdate()
    {
        currentTimer -= Time.deltaTime;
    }

    public override void OnStateEnd()
    {
    }

    public override int StateTransitionCondition()
    {
        if (currentTimer <= 0.0f)
        {
            return (int)EFaunaState.Patrolling;
        }

        return (int)EFaunaState.Invalid;
    }
}
