/// Look around script for camera movement
/**
    This script handles the camera movement based
    on the mouse input.
*/
using UnityEngine;

public class LookAround : MonoBehaviour
{
    //Public vairables defined in Unity Inspector
    public Transform player;
    public float mouseSens = 2.0f;

    //Other variables
    private float xRot = 0.0f;

    //Init
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Update
    void Update()
    {
        if(Time.timeScale == 1.0f)
        {
            //get mouse input values
            float mouseX = Input.GetAxis("Mouse X") * mouseSens;// * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSens;// * Time.deltaTime;

            xRot -= mouseY;
            //Locks rotation to avoid flipping the camera
            xRot = Mathf.Clamp(xRot, -80.0f, 80.0f);
            //Apply rotation
            transform.localRotation = Quaternion.Euler(xRot, 0.0f, 0.0f);
            player.Rotate(Vector3.up * mouseX);
        }
    }
}
