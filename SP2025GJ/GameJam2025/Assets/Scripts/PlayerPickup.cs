using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private float armLength = 1.5f;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private Transform playerPickupObj;

    private SpringJoint joint;

    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<SpringJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPickupObj.position = transform.position + playerLook.LookDirection * armLength;

        if (CheckForObj())
        {

        }
    }

    bool CheckForObj()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, playerLook.LookDirection * armLength);

        return false;
    }
}
