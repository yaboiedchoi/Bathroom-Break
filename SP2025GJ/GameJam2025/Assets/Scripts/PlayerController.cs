using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Components
    [SerializeField] CharacterController controller;

    //Fields
    [SerializeField][Range(0.01f, 10.0f)] private float movementSpeed = 5.0f; //speed of movement
    [SerializeField][Range(0.01f, 10.0f)] private float rotationSpeed = 2.0f; //mouse sensitivity
    [SerializeField] private float gravity = 9.8f; //gravity force
   // [SerializeField][Range(0.01f, 5.0f)] float jumpForce = 2; //anime reference! should be greater than gravity

    private float verticalRotation = 0.0f; //vertical rotation to prevent rolling
    private Vector3 velocity; //velocity for gravity

    void Update()
    {
        HandleMovement();
        HandleRotation();
        ApplyGravity();
    }

    void HandleMovement()
    {
        float moveX = 0f;
        float moveZ = 0f;

        //Collect Movement
        if (Input.GetKey(KeyCode.W))
        {
            moveZ = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveZ = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        //if (Input.GetKey(KeyCode.Space) && controller.isGrounded)
        //{
        //    velocity.y += gravity * jumpForce;
        //}

        //Apply Movement
        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        moveDirection.Normalize(); //prevent faster diagonal movement
        controller.Move(moveDirection * movementSpeed * Time.deltaTime);
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
        transform.Rotate(Vector3.up * mouseX);
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
    }

    void ApplyGravity()
    {
        if (!controller.isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = -1f; // Keeps player grounded
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
