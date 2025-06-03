using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameAI: MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject target;
    
   
    [Header("If true, the bot will Pursue; if false, it will Evade.")]
    [SerializeField] private bool Mode = false;
    
    void Start() {
        agent = this.GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.stoppingDistance = 0f;
    }

    public void SetMode(bool pursueMode) {
        Mode = pursueMode;
    }
    void Flee(Vector3 location) {
        Vector3 fleeLoc = this.transform.position -(location - this.transform.position);
        agent.SetDestination(fleeLoc);
    }
    void Pursue(Vector3 location){
        Vector3 targetDir = target.transform.position - this.transform.position;
        float lookAhead = targetDir.magnitude / (agent.speed + target.GetComponent<GameDrive>().currentSpeed);
        agent.SetDestination(target.transform.position + target.transform.forward * lookAhead);
    }

    void Evade(Vector3 location) {
        Vector3 targetDir = target.transform.position - this.transform.position;
        float lookAhead = targetDir.magnitude / (agent.speed + target.GetComponent<GameDrive>().currentSpeed);
        Flee(target.transform.position + target.transform.forward * lookAhead);
    }

    void Update() {
        if (!GameState.IsRunning){
            agent.isStopped = true;
            agent.ResetPath();
            return;
        }
        if (Mode){
            Pursue(target.transform.position);
            
        }else{
            
            Evade(target.transform.position);
        }
    }
}
