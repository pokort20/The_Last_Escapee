using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController playerController;
    public float movementSpeed = 10.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.0f;
    public float sprintMult = 1.5f;
    public float crouchMult = 0.75f;
    public float crouchHeight = 1.0f;
    public float standardHeight = 1.7f;
    private bool isSprinting;
    private bool isCrouching;
    private bool wasCrouching;
    Vector3 velocity;
    public Transform groundCollisionCheck;
    public float groundCollisionCheckSphereRadius = 0.2f;
    public LayerMask groundMask;
    bool isGrounded;
    private enum playerStates : int
    {
        NORMAL = 0,
        CROUCHING = 1,
        UNCROUCHING = 2,
        SPRINTING = 3
    }
    private playerStates playerState = playerStates.NORMAL;
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
        if(playerState != playerStates.UNCROUCHING)
        {
            FindPlayerState();
        }
        else
        {
            Vector3 bottomSphere = new Vector3(playerController.transform.position.x, playerController.transform.position.y - 0.5f, playerController.transform.position.z);
            Vector3 topSphere = new Vector3(playerController.transform.position.x, playerController.transform.position.y + 0.5f, playerController.transform.position.z);
            /*
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GameObject sphere1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = bottomSphere;
            sphere1.transform.position = topSphere;
            */
            LayerMask mask =~ LayerMask.GetMask("Player");
            if (Physics.CapsuleCast(bottomSphere, topSphere, 2.0f, playerController.transform.forward, mask)) 
            {
                Debug.Log("Can't uncrouch yet");
            }
            else
            {
                Debug.Log("Uncrouching");
                playerState = playerStates.NORMAL;
            }
        }
        switch (playerState)
        {
            case playerStates.NORMAL:
                //Debug.Log("Normal state");
                playerController.Move(movement * movementSpeed * Time.deltaTime);
                playerController.height = standardHeight;
                break;
            case playerStates.CROUCHING:
                //Debug.Log("Crouching state");
                playerController.Move(movement * movementSpeed * crouchMult * Time.deltaTime);
                playerController.height = crouchHeight;
                break;
            case playerStates.UNCROUCHING:
                //Debug.Log("Uncrouching state");
                break;
            case playerStates.SPRINTING:
                //Debug.Log("Sprinting state");
                playerController.Move(movement * movementSpeed * sprintMult * Time.deltaTime);
                playerController.height = standardHeight;
                break;
            default:
                //Debug.Log("Default");
                break;
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        playerController.Move(velocity * Time.deltaTime);
    }
    private void FindPlayerState()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            playerState = playerStates.CROUCHING;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            playerState = playerStates.UNCROUCHING;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            playerState = playerStates.SPRINTING;
        }
        else
        {
            playerState = playerStates.NORMAL;
        }
    }
}
