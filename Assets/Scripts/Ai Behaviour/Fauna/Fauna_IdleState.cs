using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fauna_IdleState : AStateBehaviour
{
    [Header("Idle Settings")]
    [SerializeField] private float maxTimer = 5.0f;
    [SerializeField] private int randomChanceOfStopping = 10;
    [SerializeField] private bool willRotate = true;
    
    [Header("Idle rotation Settings")]
    [SerializeField] private bool isRotating = false;
    [SerializeField] private float rotationTimer = 3f;
    [SerializeField] private float rotationSpeed = 50f;
    private float rotationDirection = 1f;
    
    [Header("Debug info")]
    [SerializeField] private float currentTimer = 0.0f;

    public override bool InitializeState()
    {
        return true;
    }

    public override void OnStateStart()
    {
        maxTimer = Random.value * 5f + 1f;
        
        currentTimer = maxTimer;
        
        //chance to stop and rotate around
        //willRotate = (int)Random.value * randomChanceOfStopping == randomChanceOfStopping;
    }

    public override void OnStateUpdate()
    {
        if (!isRotating)
            currentTimer -= Time.deltaTime;

        
        //add a spin that which direction can be inverted during update
        //checks if it should rotate and performs siad rotation
        if (willRotate && !isRotating)
        {
            StartCoroutine(RotateMf(rotationSpeed));
            
            rotationDirection *= -1f;
            
            willRotate = false;
        }
        
    }

    private IEnumerator RotateMf(float speedDegPerSec)
    {
        isRotating = true;
        float rotatedDegrees = 0f;
        int direction = 1;

        while (rotatedDegrees < 360f)
        {
            // Random chance to flip direction
            if (Random.Range(0, 21) >= 22)
                direction *= -1;

            float rotationThisFrame = direction * speedDegPerSec * Time.deltaTime;
            transform.Rotate(Vector3.up, rotationThisFrame);

            rotatedDegrees += Mathf.Abs(rotationThisFrame); // Always add positive value

            yield return null;
        }

        isRotating = false;
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
