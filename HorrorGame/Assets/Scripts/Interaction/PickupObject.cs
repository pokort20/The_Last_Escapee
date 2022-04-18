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
    private enum mouseStates : int
    {
        DEFAULT = 0,
        DOWN = 1,
        HOLDING = 2,
        UP = 3
    }
    private mouseStates mouseState;
    private Inventory inventory;
    float initGrabDistance;
    RaycastHit raycastHit;
    private bool isHoldingObject;
    GameObject holdingObject;
    LayerMask layerMask;
    private SpringJoint springJoint;
    private float initHingeAngle;
    private Vector2 initPlayerForward;
    private HingeJoint hingeJnt;
    Vector3 springAnchor;
    float circleRadius;

    //old RB values
    bool rbUseGravity;
    float rbDrag;
    float rbAngularDrag;

    void Start()
    {
        mouseState = mouseStates.DEFAULT;
        Debug.Log("Started PickupObjects");
        inventory = Inventory.instance;
        layerMask = LayerMask.GetMask("Moveable") + LayerMask.GetMask("Hinge");
        isHoldingObject = false;
        holdingObject = null;
        initGrabDistance = 0.0f;
        springJoint = new SpringJoint();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mouseState = mouseStates.DOWN;
        }
        else if(Input.GetMouseButton(0))
        {
            mouseState = mouseStates.HOLDING;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseState = mouseStates.UP;
        }
        else
        {
            mouseState = mouseStates.DEFAULT;
        }

        
        /*
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
            if (Input.GetMouseButtonUp(0) || Vector3.Distance(holdingObject.transform.position, FirstPersonCamera.transform.position) > maxGrabDistance* 1.4f)
            {
                dropObject();
            }

        }
        */
    }

    void FixedUpdate()
    {
        bool colided = false;
        if(!isHoldingObject)
        {
            colided = Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out raycastHit, maxGrabDistance, layerMask);
            if(colided)
            {
                if (!inventory.inventoryOpened)
                {
                    inventory.mouseInteracted = true;
                }
            }
            else
            {
                inventory.mouseInteracted = false; ;
            }
        }
        else
        {
            if (!inventory.inventoryOpened)
            {
                inventory.mouseInteracted = true;
            }
        }

        switch (mouseState)
        {
            case mouseStates.DOWN:
                if (colided && !isHoldingObject) pickupObject();
                break;
            case mouseStates.HOLDING:
                if (isHoldingObject)
                {
                    if (Vector3.Distance(holdingObject.transform.position, FirstPersonCamera.transform.position) > maxGrabDistance * 1.4f)
                    {
                        dropObject();
                    }
                    else
                    {
                        moveObject();
                    }
                }
                break;
            case mouseStates.UP:
                if (isHoldingObject) dropObject();
                break;
            case mouseStates.DEFAULT:
                break;
            default:
                break;
        }
    }

    private void pickupObject()
    {
        Debug.LogWarning("Picked up object");
        Debug.Log("Raycast collided with: " + raycastHit.collider.gameObject.name);
        //Debug.Log("Distance: " + Vector3.Distance(FirstPersonCamera.transform.position, raycastHit.collider.gameObject.transform.position));

        if(raycastHit.collider.gameObject.GetComponent<Rigidbody>() != null)
        {
            holdingObject = raycastHit.collider.gameObject;
            if(holdingObject.layer == LayerMask.NameToLayer("Moveable"))
            {
                isHoldingObject = true;
                setRigidbodyValues(holdingObject);
                initGrabDistance = Vector3.Distance(FirstPersonCamera.transform.position, holdingObject.transform.position);
                springJoint = holdingObject.AddComponent<SpringJoint>();
                springJoint.autoConfigureConnectedAnchor = false;
                springJoint.connectedAnchor = raycastHit.point;//FirstPersonCamera.transform.forward * initGrabDistance + FirstPersonCamera.transform.position;
                springJoint.spring = 400.0f;
                springJoint.damper = 50.0f;
            }
            else if(holdingObject.layer == LayerMask.NameToLayer("Hinge"))
            {
                //IsHoldingObject = true;
                //setRigidbodyValues(holdingObject);
                //initGrabDistance = Vector3.Distance(FirstPersonCamera.transform.position, holdingObject.transform.position);
                //hingeJnt = holdingObject.GetComponent<HingeJoint>();
                //springJoint = holdingObject.AddComponent<SpringJoint>();
                //springJoint.autoConfigureConnectedAnchor = false;
                //springJoint.spring = 40.0f;
                //springJoint.damper = 5.0f;

                //Vector3 posOnCircle = Vector3.Normalize(new Vector3(
                //    holdingObject.transform.position.x - hingeJnt.connectedAnchor.x,
                //    hingeJnt.connectedAnchor.y,
                //    holdingObject.transform.position.z - hingeJnt.connectedAnchor.z
                //    ));
                //initHingeAngle = Vector3.Angle(new Vector3(1.0f, hingeJnt.connectedAnchor.y, 0.0f), posOnCircle);
                //initHingeAngle = Vector2.SignedAngle(new Vector2(1.0f, 0.0f), new Vector2(posOnCircle.x, posOnCircle.z));
                //initPlayerForward = new Vector2(FirstPersonCamera.transform.forward.x, FirstPersonCamera.transform.forward.z);

                //springJoint.connectedAnchor = posOnCircle;
                //springJoint.anchor = new Vector3(0.0f, 0.0f, 0.0f);
                circleRadius = 0.8f * Vector2.Distance(
                    new Vector2(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.z),
                    new Vector2(raycastHit.point.x, raycastHit.point.z));
                isHoldingObject = true;
                setRigidbodyValues(holdingObject);
                hingeJnt = holdingObject.GetComponent<HingeJoint>();
                

                springAnchor = FirstPersonCamera.transform.position + circleRadius * Vector3.Normalize(new Vector3(FirstPersonCamera.transform.forward.x, hingeJnt.connectedAnchor.y, FirstPersonCamera.transform.forward.z));
                springJoint = holdingObject.AddComponent<SpringJoint>();
                springJoint.spring = 40.0f;
                springJoint.damper = 5.0f;
                springJoint.autoConfigureConnectedAnchor = false;
                springJoint.connectedAnchor = springAnchor;

            }

        }
    }
    private void moveObject()
    {
        Debug.LogWarning("Moving object");
        Vector3 anchor;
        if (holdingObject.layer == LayerMask.NameToLayer("Moveable"))
        {
            anchor = FirstPersonCamera.transform.forward * initGrabDistance + FirstPersonCamera.transform.position;
            springJoint.connectedAnchor = anchor;
        }
        else if(holdingObject.layer == LayerMask.NameToLayer("Hinge"))
        {
            Debug.Log("circle radius: " + circleRadius);
            //Vector2 playerForward = new Vector2(FirstPersonCamera.transform.forward.x, FirstPersonCamera.transform.forward.z);
            //float playerAngle = Vector2.SignedAngle(playerForward, initPlayerForward);
            //anchor = hingeJnt.connectedAnchor + new Vector3(Mathf.Cos(Mathf.Deg2Rad * (initHingeAngle + playerAngle)), hingeJnt.anchor.y, Mathf.Sin(Mathf.Deg2Rad * (initHingeAngle + playerAngle)));
            //springJoint.connectedAnchor = anchor;
            //Debug.Log("initHingeAngle: " + initHingeAngle + ", playerAngle: " + playerAngle);

            springAnchor = FirstPersonCamera.transform.position + circleRadius * Vector3.Normalize(new Vector3(FirstPersonCamera.transform.forward.x, hingeJnt.connectedAnchor.y, FirstPersonCamera.transform.forward.z));
            springJoint.connectedAnchor = springAnchor;


        }
    }
    private void dropObject()
    {
        if (holdingObject.layer == LayerMask.NameToLayer("Moveable"))
        {
            Destroy(holdingObject.GetComponent<SpringJoint>());
            setOriginalRigidbodyValues(holdingObject);
            isHoldingObject = false;
            holdingObject = null;
        }
        else if (holdingObject.layer == LayerMask.NameToLayer("Hinge"))
        {
            Destroy(holdingObject.GetComponent<SpringJoint>());
            setOriginalRigidbodyValues(holdingObject);
            isHoldingObject = false;
            holdingObject = null;


        }
        Debug.LogWarning("Released object");
    }
    private Vector3 clampVecMagnitude(Vector3 value, float clampValue)
    {
        if (Vector3.Magnitude(value) > clampValue)
        {
            return Vector3.Normalize(value) * clampValue;
        }
        return value;
    }
    private void setRigidbodyValues(GameObject gameObject)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        //save original values
        rbUseGravity = rb.useGravity;
        rbDrag = rb.drag;
        rb.angularDrag = rbAngularDrag;
        //set new values
        rb.useGravity = false;
        rb.drag = 4.0f;
        rb.angularDrag = 10.0f;
    }
    private void setOriginalRigidbodyValues(GameObject gameObject)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        //set original values
        rb.useGravity = rbUseGravity;
        rb.drag = rbDrag;
        rb.angularDrag = rbAngularDrag;
    }
}
