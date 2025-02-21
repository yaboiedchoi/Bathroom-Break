using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollEnabler : MonoBehaviour
{
    // Serialized fields
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform ragdoolRoot;
    [SerializeField]
    private bool ragdollActive = false;
    [SerializeField] AgentBehavior agentBehavior;

    // Private variables
    private CharacterJoint[] joints;
    private Collider[] colliders;

    // Public variables
    public Rigidbody[] rigidbodies;
    public float explosionForce = 500f;

    private void Awake()
    {
        // Components of game object
        rigidbodies = ragdoolRoot.GetComponentsInChildren<Rigidbody>();
        joints = ragdoolRoot.GetComponentsInChildren<CharacterJoint>();
        colliders = ragdoolRoot.GetComponentsInChildren<Collider>();
    }


    private void Update()
    {
        switch (agentBehavior.GetState())
        {
            case AgentBehavior.AgentStates.Idle:
                break;
            case AgentBehavior.AgentStates.Wander:
                EnableAnimator();
                break;
            case AgentBehavior.AgentStates.Ragdoll:
                EnableRagdoll();
                break;
        }
    }

    public void EnableRagdoll()
    {
        // Disable the animations
        animator.enabled = false;

        // Enable joint collision
        foreach (CharacterJoint joint in joints)
        {
            joint.enableCollision = true;

        }

        // Enable colliders
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;

        }

        // Enable physics based forces
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
        }

    }

    public void EnableAnimator()
    {
        animator.enabled = true;

        // Disable joint collision
        foreach (CharacterJoint joint in joints)
        {
            joint.enableCollision = false;

        }

        // Disable colliders
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;

        }

        // Disable physics based forces
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
        }
    }

    /// <summary>
    /// Shoots the rigid body into the sky
    /// </summary>
    public void ApplyExplosionForce()
    {
        // Can't explode while animations are active
        if (!ragdollActive) return;

        // Apply force in global up direction
        Vector3 upForce = Vector3.up * explosionForce;

        // Applies force to rigid bocy
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.AddForce(upForce, ForceMode.Impulse);
        }
    }

    ///// <summary>
    ///// Controls for enabling, disabling animations and ragdoll and button to apply upward force on gameobject
    ///// </summary>
    //private void OnGUI()
    //{
    //    if (GUI.Button(new Rect(10, 10, 150, 50), "Enable Ragdoll"))
    //    {
    //        ragdollActive = true;
    //        EnableRagdoll();
    //    }

    //    if (GUI.Button(new Rect(10, 70, 150, 50), "Enable Animator"))
    //    {
    //        ragdollActive = false;
    //        EnableAnimator();
    //    }

    //    if (GUI.Button(new Rect(10, 130, 150, 50), "Explode"))
    //    {
    //        ApplyExplosionForce();
    //    }
    //}
}
