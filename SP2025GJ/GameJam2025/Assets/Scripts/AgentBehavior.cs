using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentBehavior : MonoBehaviour
{
    [SerializeField] Transform playerTransform; //destination of agent
    [SerializeField] NavMeshAgent agent; //agent component

    /// <summary>
    /// all possible states an agent can be in
    /// </summary>
    public enum AgentStates
    {
        Idle,
        Wander,
        Ragdoll,
    }

    //character starts in wander
    AgentStates currentState;

    private void Start()
    {
        currentState = AgentStates.Wander;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case AgentStates.Idle:
                //happen when agent is in its designated seat/position
                break;
            case AgentStates.Wander:
                //currently has agent follow player, will change
              agent.SetDestination(playerTransform.position);
                break;
            case AgentStates.Ragdoll:
                //happens when the player picks up/punches the agent
                agent.SetDestination(this.gameObject.transform.position);
                break;
        }

        Debug.Log("Agent State: " + currentState);
    }

    /// <summary>
    /// Changes the agent's state 
    /// </summary>
    /// <param name="newSate">new state for agent to be put in</param>
    public void SetState(AgentStates newSate)
    {
        currentState = newSate;
    }
}
