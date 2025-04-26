using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLUG_ReactState : AStateBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerDetectionTrigger playerDetection;
    
    [Header("Trigger")]
    [SerializeField] private SphereCollider sphereCollider;
    
    [Header("Settings")]
    [SerializeField] private float timeToAssureSafety = 1.0f;
    
    [Header("Debug")]
    [SerializeField] private float currentTimer = 0.0f;

    public override bool InitializeState()
    {
        if (sphereCollider == null)
        {
            Debug.LogWarning($"SLUG_ReactState on {gameObject.name} is missing references!");
            return false;
        }
        return true;
    }

    public override void OnStateStart()
    {
        currentTimer = timeToAssureSafety;
    }

    public override void OnStateUpdate()
    {
        if (!playerDetection.IsPlayerInside)
        {
            currentTimer -= Time.deltaTime;
        }
        else
        {
            currentTimer = timeToAssureSafety;
        }
    }

    public override void OnStateEnd()
    {
    }

    public override int StateTransitionCondition()
    {
        if (currentTimer <= 0.0f)
        {
            return (int)EFaunaState.Idle;
        }

        return (int)EFaunaState.Invalid;
    }
}
