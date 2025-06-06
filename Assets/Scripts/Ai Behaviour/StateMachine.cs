using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private List<AStateBehaviour> stateBehaviours = new List<AStateBehaviour>();

    [SerializeField] private int defaultState = 0;

    private AStateBehaviour currentState = null;

    [SerializeField] private string currStateString = null;
    
    bool InitializeStates()
    {
        for (int i = 0; i < stateBehaviours.Count; ++i)
        {
            AStateBehaviour stateBehaviour = stateBehaviours[i];

            if (stateBehaviour)
            {
                //Debug.Log($"Initializing state: {stateBehaviour.GetType().Name}");
                
                stateBehaviour.AssociatedStateMachine = this;

                if (stateBehaviour.InitializeState())
                    continue;
            }

            Debug.Log($"StateMachine on {gameObject.name} has failed to initialize the state {stateBehaviours[i]?.GetType().Name}!");
            return false;
        }

        return true;
    }

    void Start()
    {
        if (!InitializeStates())
        {
            // Stop This class from executing
            this.enabled = false;
            return;
        }
        
        if (stateBehaviours.Count > 0)
        {
            int firstStateIndex = defaultState < stateBehaviours.Count ? defaultState : 0;

            currentState = stateBehaviours[firstStateIndex];
            currentState.OnStateStart();
            
            currStateString = currentState.GetType().Name;
        }
        else
        {
            Debug.Log($"StateMachine On {gameObject.name} is has no state behaviours associated with it!");
        }
    }

    void Update()
    {
        currentState.OnStateUpdate();

        int newState = currentState.StateTransitionCondition();
        if (IsValidNewStateIndex(newState))
        {
            string startingState = currentState.GetType().Name;

            currentState.OnStateEnd();
            currentState = stateBehaviours[newState];
            currentState.OnStateStart();
            
            Debug.Log($"{gameObject.name} transition condition met, going from {startingState} to {currentState?.GetType().Name}");
            currStateString = currentState.GetType().Name;
        }
    }

    public bool IsCurrentState(AStateBehaviour stateBehaviour)
    {
        return currentState == stateBehaviour;
    }

    public void SetState(int index)
    {
        if (IsValidNewStateIndex(index))
        {
            currentState.OnStateEnd();
            currentState = stateBehaviours[index];
            currentState.OnStateStart();
        }
    }

    private bool IsValidNewStateIndex(int stateIndex)
    {
        return stateIndex < stateBehaviours.Count && stateIndex >= 0;
    }

    public AStateBehaviour GetCurrentState()
    {
        return currentState;
    }
}
