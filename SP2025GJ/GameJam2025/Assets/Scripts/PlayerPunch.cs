using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using QFSW;
using QFSW.QC;

public class PunchObject : MonoBehaviour
{
    [SerializeField] Camera playerCamera; //camera component attached to player obj (set in editor)
    [SerializeField] private PlayerStats playerStats;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) //Right Click = 1
        {
            Punch();
        }
    }
    [Command]
    void Punch()
    {
        //Used to store info about raycast
        //Going to be used for applying force at specific pos on ragdoll
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, playerStats.punchRange))
        {
            if (hit.rigidbody != null) //check if hit obj has a rigidbody
            {
                Vector3 hitPoint = hit.point;
                Vector3 forceDirection = hitPoint - transform.position; //Direction from player to hitpoint
                forceDirection = forceDirection.normalized; //Normalize the direction

                //Apply hit to rigid body at single point
                hit.rigidbody.AddForceAtPosition(forceDirection * playerStats.punchForce, hitPoint, ForceMode.Impulse);
            }

            //check if the object is an agent
            AgentBehavior agent = hit.collider.GetComponentInParent<AgentBehavior>();
            if (agent != null) //ensure the component exists
            {
                //set the agent to ragdoll
                agent.SetState(AgentBehavior.AgentStates.Ragdoll);
            }
        }
    }
}
