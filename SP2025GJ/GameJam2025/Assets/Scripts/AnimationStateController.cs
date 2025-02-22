using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AgentBehavior;

public class NewBehaviourScript : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int isIdleHash;
    int isSittingHash;
    int isCrawlingHash;

    public enum AnimationState
    {
        Idle,
        Walk,
        Run,
        Sit,
        Crawl
    }

    AnimationState currentState;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isIdleHash = Animator.StringToHash("isIdle");
        isSittingHash = Animator.StringToHash("isSitting");
        isCrawlingHash = Animator.StringToHash("isCrawling");

        currentState = AnimationState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        // Placeholder conditions to change states
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeState(AnimationState.Walk);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeState(AnimationState.Sit);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeState(AnimationState.Crawl);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeState(AnimationState.Run);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState(AnimationState.Idle);
        }
    }

    /// <summary>
    /// Changes the state to the new animation state
    /// </summary>
    /// <param name="newState">The state being switched too</param>
    void ChangeState(AnimationState newState)
    {
        // Don't need to check states if the current state is the new state
        if (currentState == newState) return; 

        // Reset all animator states
        animator.SetBool(isWalkingHash, false);
        animator.SetBool(isRunningHash, false);
        animator.SetBool(isIdleHash, false);
        animator.SetBool(isSittingHash, false);
        animator.SetBool(isCrawlingHash, false);

        // Set the new state
        switch (newState)
        {
            case AnimationState.Idle:
                animator.SetBool(isIdleHash, true);
                break;
            case AnimationState.Walk:
                animator.SetBool(isWalkingHash, true);
                break;
            case AnimationState.Run:
                animator.SetBool(isRunningHash, true);
                break;
            case AnimationState.Sit:
                animator.SetBool(isSittingHash, true);
                break;
            case AnimationState.Crawl:
                animator.SetBool(isCrawlingHash, true);
                break;
        }

        currentState = newState;
    }
}