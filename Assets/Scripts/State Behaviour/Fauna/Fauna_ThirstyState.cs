using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fauna_ThirstyState : AStateBehaviour
{
    [SerializeField] private float drinkTimer = 1.0f;
    float currentTimer = 0.0f;

    public override bool InitializeState()
    {
        return true;
    }

    public override void OnStateStart()
    {
        currentTimer = drinkTimer;
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
            return (int)EFaunaState.Wander;
        }

        return (int)EFaunaState.Invalid;
    }
}
