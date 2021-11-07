using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    //values set in inspector
    public Transform holdingPosition;
    public Camera FirstPersonCamera;
    public float maxGrabDistance;
    public float grabSpeedMultiplier;

    //other variables
    float grabDistance;
    float moveDistance;
    float holdingObjectDefaultDrag;
    RaycastHit raycastHit;
    bool IsHoldingObject;
    GameObject holdingObject;
    Vector3 previousPosition;
    Vector3 moveForce;

    void Start()
    {
        Debug.Log("Started PickupObjects");
        IsHoldingObject = false;
        holdingObject = null;
        grabDistance = 0.0f;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("LMB Down");
            if (Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out raycastHit, maxGrabDistance))
            {
                Debug.Log("Fired raycast");
                if (raycastHit.collider.gameObject.GetComponent<Rigidbody>() != null)
                {
                    holdingObject = raycastHit.collider.gameObject;
                    previousPosition = holdingObject.transform.position;
                    holdingObject.GetComponent<Rigidbody>().useGravity = false;
                    holdingObjectDefaultDrag = holdingObject.GetComponent<Rigidbody>().drag;
                    //holdingObject.GetComponent<Rigidbody>().isKinematic = true;
                    grabDistance = Vector3.Distance(FirstPersonCamera.transform.position, holdingObject.transform.position);
                    Debug.Log("Distance: " + grabDistance);
                    Debug.Log("Direciton: " + FirstPersonCamera.transform.forward);
                    Debug.Log("Found rigidbody object! Object:" + holdingObject.name);
                    IsHoldingObject = true;
                }
            }
        }
        if (IsHoldingObject)
        {
            if (Input.GetMouseButton(0))
            {
                //update holding object's pos
                //holdingObject.transform.position = holdingPosition.position;
                //holdingObject.transform.position = FirstPersonCamera.transform.forward * grabDistance + FirstPersonCamera.transform.position;
                moveForce = (FirstPersonCamera.transform.forward * grabDistance + FirstPersonCamera.transform.position) - previousPosition;
                moveDistance = Vector3.Distance(FirstPersonCamera.transform.forward * grabDistance + FirstPersonCamera.transform.position, previousPosition);
                moveForce *= grabSpeedMultiplier * moveDistance;
                Debug.Log("moveForce: " + moveForce);
                holdingObject.GetComponent<Rigidbody>().AddForce(moveForce);
                holdingObject.GetComponent<Rigidbody>().drag = 1/moveDistance;
                previousPosition = holdingObject.transform.position;
            }
            if (Input.GetMouseButtonUp(0))
            {
                //holdingObject.GetComponent<Rigidbody>().isKinematic = false;
                holdingObject.GetComponent<Rigidbody>().useGravity = true;
                holdingObject.GetComponent<Rigidbody>().drag = holdingObjectDefaultDrag;
                IsHoldingObject = false;
                holdingObject = null;
                Debug.Log("Released object");
            }
            
        }
    }
}
