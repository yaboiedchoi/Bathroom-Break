using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentBehavior : MonoBehaviour
{
    [SerializeField]Transform playerTransform; //destination of agent
    [SerializeField]NavMeshAgent agent; //agent component

    public enum AgentStates 
    { 
        Wander,
        Ragdoll
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(playerTransform.position);      
    }
}
