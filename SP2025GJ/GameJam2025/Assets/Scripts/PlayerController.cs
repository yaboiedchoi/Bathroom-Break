using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform player; //player's transform
    [SerializeField][Range(0.01f, 10.0f)] float movementSpeed = 1.0f; //speed to be scaled by deltaTime

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            player.position += new Vector3(movementSpeed, 0, 0) * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            player.position += new Vector3(-movementSpeed, 0, 0) * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            player.position += new Vector3(0, 0, -movementSpeed) * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            player.position += new Vector3(0, 0, movementSpeed) * Time.deltaTime;
        }
    }
}
