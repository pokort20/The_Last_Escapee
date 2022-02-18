using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    //values set in inspector
    public Transform holdingPosition;
    public Camera FirstPersonCamera;
    public float forceAmount = 9.81f;
    public float maxGrabDistance;
    public float grabSpeedMultiplier;
    public float toCenterParam;

    //other variables
    float initGrabDistance;
    float grabDistance;
    float moveDistance;
    float holdingObjectDefaultDrag;
    float holdingObjectDefaultAngularDrag;
    RaycastHit raycastHit;
    bool IsHoldingObject;
    GameObject holdingObject;
    Vector3 previousPosition;
    Vector3 targetPosition;
    Vector3 moveDirection;
    LayerMask layerMask;

    void Start()
    {
        Debug.Log("Started PickupObjects");
        layerMask = LayerMask.GetMask("Moveable");
        IsHoldingObject = false;
        holdingObject = null;
        initGrabDistance = 0.0f;
    }
    void Update()
    {
        //Debug.Log("Is Holding Object: " + IsHoldingObject.ToString());
        if (Input.GetMouseButtonDown(0))
        {
            pickupObject();
        }
        if (IsHoldingObject)
        {
            if (Input.GetMouseButton(0))
            {
                moveObject();
            }
            if (Input.GetMouseButtonUp(0) || Vector3.Distance(holdingObject.transform.position, FirstPersonCamera.transform.position) > maxGrabDistance)
            {
                dropObject();
            }

        }
    }

    private void pickupObject()
    {
        if (Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out raycastHit, maxGrabDistance, layerMask))
        {
            Debug.Log("Raycast collided with: " + raycastHit.collider.gameObject.name);
            Debug.Log("Distance: " + Vector3.Distance(FirstPersonCamera.transform.position, raycastHit.collider.gameObject.transform.position));

            if (raycastHit.collider.gameObject.GetComponent<Rigidbody>() != null)
            {
                holdingObject = raycastHit.collider.gameObject;
                previousPosition = holdingObject.transform.position;
                holdingObject.GetComponent<Rigidbody>().useGravity = false;
                holdingObjectDefaultDrag = holdingObject.GetComponent<Rigidbody>().drag;
                holdingObjectDefaultAngularDrag = holdingObject.GetComponent<Rigidbody>().angularDrag;
                initGrabDistance = Vector3.Distance(FirstPersonCamera.transform.position, holdingObject.transform.position);
                Debug.Log("Distance: " + initGrabDistance);
                Debug.Log("Direciton: " + FirstPersonCamera.transform.forward);
                Debug.Log("Found rigidbody object! Object:" + holdingObject.name);
                IsHoldingObject = true;

                targetPosition = FirstPersonCamera.transform.forward * initGrabDistance + FirstPersonCamera.transform.position;
            }
        }
    }
    private void moveObject()
    {
        //update holding object's pos
        //old method, using set position
        //holdingObject.transform.position = FirstPersonCamera.transform.forward * grabDistance + FirstPersonCamera.transform.position;
        targetPosition = FirstPersonCamera.transform.forward * initGrabDistance + FirstPersonCamera.transform.position;

        moveDirection = targetPosition - previousPosition; //!
        moveDistance = Vector3.Distance(targetPosition, previousPosition);
        Debug.Log("move distance: " + moveDistance + ", maxGrabDistance: " + maxGrabDistance);
        if(moveDistance < maxGrabDistance)
        {
            Vector3 appliedForce = moveDirection * forceAmount * Time.deltaTime;
            Debug.Log("Applying force with magnitude: " + appliedForce.magnitude + ", force direction: " + appliedForce);
            holdingObject.GetComponent<Rigidbody>().drag = 1 / (moveDistance * moveDistance);
            holdingObject.GetComponent<Rigidbody>().angularDrag = 1 / moveDistance;
            holdingObject.GetComponent<Rigidbody>().AddForce(appliedForce);
        }
        previousPosition = holdingObject.transform.position;
        /*
        moveDistance = Vector3.Distance(targetPosition, previousPosition);
        targetPosition -= FirstPersonCamera.transform.position;
        targetPosition *= toCenterParam * moveDistance;
        moveDirection = targetPosition - previousPosition;
        moveDirection *= grabSpeedMultiplier * moveDistance;
        Debug.Log("vec magnitude: " + Vector3.Magnitude(clampVecMagnitude(moveDirection, 150.0f)) + " moveForce: " + clampVecMagnitude(moveDirection, 150.0f) + "velocity magnitude: " + Vector3.Magnitude(holdingObject.GetComponent<Rigidbody>().velocity)  + " moveDistance: " + moveDistance);
        holdingObject.GetComponent<Rigidbody>().AddForce(clampVecMagnitude(moveDirection, 150.0f) - holdingObject.GetComponent<Rigidbody>().velocity * 15.0f);
        holdingObject.GetComponent<Rigidbody>().drag = 1 / (moveDistance * moveDistance);
        holdingObject.GetComponent<Rigidbody>().angularDrag = 1 / moveDistance;
        previousPosition = holdingObject.transform.position;
        */
    }
    private void dropObject()
    {
        //holdingObject.GetComponent<Rigidbody>().isKinematic = false;
        holdingObject.GetComponent<Rigidbody>().useGravity = true;
        holdingObject.GetComponent<Rigidbody>().drag = holdingObjectDefaultDrag;
        holdingObject.GetComponent<Rigidbody>().angularDrag = holdingObjectDefaultAngularDrag;

        IsHoldingObject = false;
        holdingObject = null;
        Debug.Log("Released object");
    }
    private Vector3 clampVecMagnitude(Vector3 value, float clampValue)
    {
        if (Vector3.Magnitude(value) > clampValue)
        {
            return Vector3.Normalize(value) * clampValue;
        }
        return value;
    }
}
