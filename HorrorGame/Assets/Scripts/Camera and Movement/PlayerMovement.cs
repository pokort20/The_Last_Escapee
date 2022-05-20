/// Player movement class
/**
    This class handles the player movement. It uses the keyboard input
    and player's facing direction to compute the resulting movement vector.
    Based on the state the player is in, it scales the vector. This vector
    is then passed to the character controller component.
*/
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{
    //Public variables defined in Unity inspector
    public CharacterController playerController;
    public float gravity;
    public float jumpHeight;
    public float sprintMult;
    public float crouchMult;
    public float crouchHeight;
    public float standardHeight;
    public Transform groundCollisionCheck;
    public Transform uncrouchCollisionCheck;
    public float groundCollisionCheckSphereRadius = 0.15f;
    public LayerMask groundMask;
    public LayerMask uncrouchMask;

    //Other variables
    private float movementSpeed;
    private bool isSprinting;
    private Vector3 moveVec;
    private PostProcessing postProcessing;
    private float uncrouchCollisionCheckSphereRadius;
    private float stepOffset;

    Vector3 velocity;
    bool isGrounded;
    //States
    private enum playerStates : int
    {
        NORMAL = 0,
        CROUCHING = 1,
        UNCROUCHING = 2,
        SPRINTING = 3
    }
    private playerStates playerState = playerStates.NORMAL;

    //Init
    void Start()
    {
        movementSpeed = GameManager.instance.movementSpeed;
        gravity = -19.62f;
        jumpHeight = 0.7f;
        sprintMult = 1.5f;
        crouchMult = 0.75f;
        crouchHeight = 1.0f;
        standardHeight = 1.7f;
        stepOffset = 0.3f;
        isSprinting = false;
        postProcessing = PostProcessing.instance;
        uncrouchCollisionCheckSphereRadius = 0.3f;

        Invoke("displayMovementHelp", 1.5f);
    }

    //Update
    void Update()
    {
        //Helper sphere to check if player is grounded
        isGrounded = Physics.CheckSphere(groundCollisionCheck.position, groundCollisionCheckSphereRadius, groundMask);

        if (isGrounded && velocity.y < 0.0f)
        {
            velocity.y = -2.2f;
            playerController.stepOffset = stepOffset;
        }

        //compute movement direction
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveVec = transform.right * horizontal + transform.forward * vertical;
        if (moveVec.magnitude > 1.0f)
        {
            moveVec.Normalize();
        }

        //handle uncrouching state
        if (playerState != playerStates.UNCROUCHING)
        {
            FindPlayerState();
        }
        else
        {
            Vector3 bottomSphere = new Vector3(playerController.transform.position.x, playerController.transform.position.y - 0.5f, playerController.transform.position.z);
            Vector3 topSphere = new Vector3(playerController.transform.position.x, playerController.transform.position.y + 0.5f, playerController.transform.position.z);

            if (Physics.CheckSphere(uncrouchCollisionCheck.position, radius: uncrouchCollisionCheckSphereRadius, uncrouchMask))
            {
                //Debug.Log("Can't uncrouch yet");
            }
            else
            {
                playerState = playerStates.NORMAL;
            }
        }

        //handle speed and other stuff based on player's state
        isSprinting = false;
        switch (playerState)
        {
            case playerStates.NORMAL:
                //Debug.Log("Normal state");
                moveVec *= movementSpeed;
                playerController.height = standardHeight;
                postProcessing.lensDistortionEnabled = false;
                postProcessing.motionBlurEnabled = false;
                break;
            case playerStates.CROUCHING:
                //Debug.Log("Crouching state");
                moveVec *= movementSpeed * crouchMult;
                playerController.height = crouchHeight;
                postProcessing.lensDistortionEnabled = false;
                postProcessing.motionBlurEnabled = false;
                break;
            case playerStates.UNCROUCHING:
                //Debug.Log("Uncrouching state");
                postProcessing.lensDistortionEnabled = false;
                postProcessing.motionBlurEnabled = false;
                break;
            case playerStates.SPRINTING:
                //Debug.Log("Sprinting state");
                moveVec *= movementSpeed * sprintMult;
                playerController.height = standardHeight;
                isSprinting = true;
                if(moveVec.magnitude != 0.0)
                {
                    postProcessing.lensDistortionEnabled = true;
                    postProcessing.motionBlurEnabled = true;
                }
                break;
            default:
                //Debug.Log("Default");
                break;
        }
        playerController.Move(moveVec * Time.deltaTime);

        //handle audio
        AudioManager.instance.movementVec = playerController.velocity;
        AudioManager.instance.isGrounded = isGrounded;

        //add jump force if player jumps
        if (Input.GetButtonDown("Jump") && isGrounded && playerState != playerStates.CROUCHING)
        {
            stepOffset = playerController.stepOffset;
            playerController.stepOffset = 0.0f;
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);

        //update stamina
        updateStamina();
    }

    //Other functions
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
        else if (Input.GetKey(KeyCode.LeftShift) && canSprint())
        {
            playerState = playerStates.SPRINTING;
        }
        else
        {
            playerState = playerStates.NORMAL;
        }
    }
    public Vector3 getPlayerMoveVec()
    {
        return moveVec;
    }
    private bool canSprint()
    {
        if(isSprinting && GameManager.instance.stamina > 0.0f)
        {
            return true;
        }
        else if(GameManager.instance.stamina > 5.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void updateStamina()
    {
        if(playerState == playerStates.SPRINTING)
        {
            GameManager.instance.stamina -= 1.0f * Time.deltaTime;
        }
        else if(playerState == playerStates.CROUCHING || playerState == playerStates.UNCROUCHING)
        {
            GameManager.instance.stamina += 0.7f * Time.deltaTime;
        }
        else
        {
            GameManager.instance.stamina += 0.6f * Time.deltaTime;
        }

        if(GameManager.instance.stamina > GameManager.instance._maxStamina)
        {
            GameManager.instance.stamina = GameManager.instance._maxStamina;
        }
        if(GameManager.instance.stamina < 0.0f)
        {
            GameManager.instance.stamina = 0.0f;
            if(Tutorial.instance != null && Inventory.instance != null && Inventory.instance.hasItem(typeof(StaminaShotItem)) > 0)
            {
                Tutorial.instance.showHelp("staminaShot");
            }
        }
    }
    private void displayMovementHelp()
    {
        if (Tutorial.instance != null)
        {
            Tutorial.instance.showHelp("movement");
        }
    }
}
