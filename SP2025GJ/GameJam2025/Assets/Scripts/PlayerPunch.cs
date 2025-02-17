using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PunchObject : MonoBehaviour
{
    [SerializeField] Camera playerCamera; //camera component attached to player obj (set in editor)
    [SerializeField][Range(1.0f, 100.0f)] float punchForce = 10.0f; //force applied to punched objects
    [SerializeField][Range(1.0f, 20.0f)] float punchRange = 2.0f; //Range for raycast

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) //Right Click = 1
        {
            Punch();
        }
    }

    void Punch()
    {
        //Used to store info about raycast
        //Going to be used for applying force at specific pos on ragdoll
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, punchRange))
        {
            if (hit.rigidbody != null) //check if hit obj has a rigidbody
            {
                Vector3 hitPoint = hit.point;
                Vector3 forceDirection = hitPoint - transform.position; //Direction from player to hitpoint
                forceDirection = forceDirection.normalized; //Normalize the direction

                //Apply hit to rigid body at single point
                hit.rigidbody.AddForceAtPosition(forceDirection * punchForce, hitPoint, ForceMode.Impulse);
            }
        }
    }
}
