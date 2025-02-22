using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AgentBehavior : MonoBehaviour
{
    [Header("Agent Components")]
    [SerializeField] NavMeshAgent agent; //agent component

    [Header("Ragdoll Transition")]
    private Vector3 prevPos;
    bool isTrackingStillness;
    private float timeStill; //time agent has not been moving for
    [SerializeField] float wakeUpTime = 3.0f; // time to wait before waking up (changing states) 
    [SerializeField][Range(0.01f, 1.0f)] float minimumDistance = 0.2f; //distance traveled to start transitioning out of ragdoll

    [Header("Wander Behavior")]
    [SerializeField] float wanderRadius = 10.0f;
    [SerializeField] float wanderChangeTime = 5.0f;
    private float wanderTimer;
    private Vector3 currentDestination;



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
        isTrackingStillness = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            //happen when agent is in its designated seat/position
            case AgentStates.Idle:
                break;
            //occurs when teacher leaves room
            case AgentStates.Wander:
                WanderBehavior();
                break;
            //happens when the player picks up/punches the agent
            case AgentStates.Ragdoll:
                TrackRagdollBehavior();
                break;
        }

        prevPos = transform.position;
    }

    /// <summary>
    /// Changes the agent's state 
    /// </summary>
    /// <param name="newSate">new state for agent to be put in</param>
    public void SetState(AgentStates newSate)
    {
        Debug.Log($"Changing from {currentState} to {newSate}");
        currentState = newSate;
        //reset tracking variables
        isTrackingStillness = false;
        timeStill = 0.0f;
    }

    public AgentStates GetState()
    {
        return currentState;
    }

    private void TrackRagdollBehavior()
    {
        float changeInPos = Vector3.Distance(prevPos, transform.position);
        //have we been kind of still?
        if (changeInPos < minimumDistance)
        {
            //are we already tracking?
            if (!isTrackingStillness)
            {
                //start tracking
                isTrackingStillness = true;
                timeStill = 0.0f;
            }

            //increment time
            timeStill += Time.deltaTime;

            //have we been still long enough?
            if (timeStill >= wakeUpTime)
            {
                //change state
                SetState(AgentStates.Wander);
            }
        }
        //if we moved
        else
        {
            //stop/dont track
            isTrackingStillness = false;
            timeStill = 0.0f;
        }
    }
    /// <summary>
    /// Wandering logic: Pick a new random destination every few seconds.
    /// </summary>
    private void WanderBehavior()
    {
        // Move agent towards current destination
        agent.SetDestination(currentDestination);

        // Update the wander timer
        wanderTimer += Time.deltaTime;

        // If it's time to pick a new destination, reset timer and choose a new point
        if (wanderTimer >= wanderChangeTime)
        {
            PickRandomDestination();
            wanderTimer = 0f;
        }
    }

    /// <summary>
    /// Picks a new random destination within the wander radius.
    /// </summary>
    private void PickRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position; // Offset from the agent's current position

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, NavMesh.AllAreas))
        {
            currentDestination = navHit.position;
            Debug.Log($"New wander destination: {currentDestination}");
        }
    }
}
