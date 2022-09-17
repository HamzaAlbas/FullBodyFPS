using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float playerSpeed = 12f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;
    private bool midAir;
    private bool isRunning;

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (!isGrounded && !isRunning)
        {
            midAir = true;
            playerSpeed = Mathf.Lerp(6f, 2f, 1f);
        }
        else if(!isGrounded && isRunning)
        {
            midAir = true;
            playerSpeed = Mathf.Lerp(10f, 2f, 1f);
        }
        else if(isGrounded && !isRunning)
        {
            midAir = false;
            playerSpeed = Mathf.Lerp(2f, 6f, 1f);
        }
        else if (isGrounded && isRunning)
        {
            midAir = false;
            playerSpeed = Mathf.Lerp(2f, 10f, 1f);
        }

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * playerSpeed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        if(Input.GetButton("Sprint") && Input.GetAxis("Vertical") > 0 && isGrounded)
        {
            isRunning = true;
            playerSpeed = Mathf.Lerp(6f, 15f, 1f);
        }
        else
        {
            isRunning = false;
            playerSpeed = Mathf.Lerp(15f, 6f, 1f);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
