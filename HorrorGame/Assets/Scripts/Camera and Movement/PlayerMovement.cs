using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController playerController;
    public float movementSpeed = 10.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.0f;
    Vector3 velocity;
    public Transform groundCollisionCheck;
    public float groundCollisionCheckSphereRadius = 0.2f;
    public LayerMask groundMask;
    bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCollisionCheck.position, groundCollisionCheckSphereRadius, groundMask);

        if (isGrounded && velocity.y < 0.0f) velocity.y = -1.5f;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * horizontal + transform.forward * vertical;

        playerController.Move(movement * movementSpeed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        playerController.Move(velocity * Time.deltaTime);
    }
}
