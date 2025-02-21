using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private Transform playerPickupObj;
    [SerializeField] private LayerMask interactable;

    private PlayerInteract playerInteract;
    private SpringJoint joint;
    private Rigidbody connectedRB;

    public bool HoldingSomething => connectedRB != null;

    // Start is called before the first frame update
    void Start()
    {
        connectedRB = null;
        joint = GetComponentInChildren<SpringJoint>();
        playerInteract = GetComponentInChildren<PlayerInteract>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPickupObj.position = transform.position + playerLook.LookDirection * stats.interactDistance;

        CheckForHover();

        if (Input.GetMouseButtonDown(0))
        {
            if (connectedRB != null)
            {
                connectedRB.freezeRotation = false;
                joint.connectedBody = null;
                connectedRB = null;
            }
            if (CheckForObj())
            {
                Debug.Log("Clicked");
                joint.connectedBody = connectedRB;
                connectedRB.freezeRotation = true;
                playerInteract.SetState(CursorStatus.Clicked);
            }
        }
    }

    bool CheckForHover()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, playerLook.LookDirection);

        if (RayCast(ray, out hit, interactable, stats))
        {
            GameObject obj = hit.collider.gameObject;

            if (obj.GetComponentInChildren<Rigidbody>() != null)
            {
                playerInteract.SetState(CursorStatus.Hovering);
                return true;
            }
            else
            {
                while (obj.transform.parent != null)
                {
                    obj = obj.transform.parent.gameObject;

                    if (obj.GetComponent<Rigidbody>() != null)
                        return true;
                }
            }
        }

        playerInteract.SetState(CursorStatus.None);
        return false;
    }

    bool CheckForObj()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, playerLook.LookDirection);

        if (RayCast(ray, out hit, interactable, stats))
        {
            GameObject obj = hit.collider.gameObject;

            connectedRB = obj.GetComponentInChildren<Rigidbody>();
            if (connectedRB != null)
            {
                playerInteract.SetState(CursorStatus.Hovering);
                return true;
            }
            else
            {
                while (obj.transform.parent != null && connectedRB == null)
                {
                    obj = obj.transform.parent.gameObject;
                    connectedRB = obj.GetComponent<Rigidbody>();
                    
                    if (connectedRB != null)
                        return true;
                }
            }
        }

        playerInteract.SetState(CursorStatus.None);
        return false;
    }

    bool RayCast(Ray ray, out RaycastHit hit, LayerMask interactable, PlayerStats stats)
    {
        return Physics.Raycast(ray, out hit, stats.interactDistance, interactable);
    }
}
