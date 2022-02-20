using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround : MonoBehaviour
{
    public Transform player;
    public float mouseSens = 2.0f;
    float xRot = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 1.0f)
        {
            //if (Cursor.lockState != CursorLockMode.Locked)
            //{
            //    Cursor.lockState = CursorLockMode.Locked;
            //}
            //get mouse input values
            float mouseX = Input.GetAxis("Mouse X") * mouseSens;// * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSens;// * Time.deltaTime;

            xRot -= mouseY;
            xRot = Mathf.Clamp(xRot, -80.0f, 80.0f);
            transform.localRotation = Quaternion.Euler(xRot, 0.0f, 0.0f);
            player.Rotate(Vector3.up * mouseX);
        }
    }
}
