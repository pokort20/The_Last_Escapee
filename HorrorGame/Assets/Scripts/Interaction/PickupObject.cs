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
    private GameObject holder;
    private Transform oldParent;
    private bool mouseDown;
    private bool mouseHold;
    private Rigidbody holdingObjectRB;
    private float currentDistance;

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
        Debug.Log("Started PickupObjects");
        inventory = Inventory.instance;
        layerMask = LayerMask.GetMask("Moveable") + LayerMask.GetMask("Hinge");
        isHoldingObject = false;
        holdingObject = null;
        initGrabDistance = 0.0f;
        springJoint = new SpringJoint();

        mouseDown = false;
        mouseHold = false;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDown = true;
        }
        else
        {
            mouseDown = false;
        }
        if(Input.GetMouseButton(0))
        {
            mouseHold = true;
        }
        else
        {
            mouseHold = false;
        }

        bool colided = false;
        if (!isHoldingObject)
        {
            if (Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out raycastHit, maxGrabDistance))
            {
                if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Moveable") || raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Hinge"))
                {
                    colided = true;
                }
            }
            if (colided)
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

        if (isHoldingObject)
        {
            if (mouseHold)
            {
                moveObject();
            }
            else
            {
                dropObject();
            }
        }
        else
        {
            if (mouseDown && colided)
            {
                pickupObject();
            }
        }
    }

    void FixedUpdate()
    {
        //bool colided = false;
        //if(!isHoldingObject)
        //{
        //    if(Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out raycastHit, maxGrabDistance))
        //    {
        //        if(raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Moveable") || raycastHit.collider.gameObject.layer  == LayerMask.NameToLayer("Hinge"))
        //        {
        //            colided = true;
        //        }
        //    }
        //    if(colided)
        //    {
        //        if (!inventory.inventoryOpened)
        //        {
        //            inventory.mouseInteracted = true;
        //        }
        //    }
        //    else
        //    {
        //        inventory.mouseInteracted = false; ;
        //    }
        //}
        //else
        //{
        //    if (!inventory.inventoryOpened)
        //    {
        //        inventory.mouseInteracted = true;
        //    }
        //}

        //if(isHoldingObject)
        //{
        //    if(mouseHold)
        //    {
        //        moveObject();
        //    }
        //    else
        //    {
        //        dropObject();
        //    }
        //}
        //else
        //{
        //    if(mouseDown && colided)
        //    {
        //        pickupObject();
        //    }
        //}
    }

    private void pickupObject()
    {
        Debug.Log("Raycast collided with: " + raycastHit.collider.gameObject.name);
        if(raycastHit.collider.gameObject.GetComponent<Rigidbody>() != null)
        {
            holdingObject = raycastHit.collider.gameObject;
            if(holdingObject.layer == LayerMask.NameToLayer("Moveable"))
            {
                isHoldingObject = true;
                holder = new GameObject();
                initGrabDistance = Vector3.Distance(FirstPersonCamera.transform.position, holdingObject.transform.position);
                holder.transform.position = FirstPersonCamera.transform.position + FirstPersonCamera.transform.forward * initGrabDistance;
                oldParent = holdingObject.transform.parent;
                holdingObject.transform.parent = holder.transform;
                holdingObjectRB = holdingObject.GetComponent<Rigidbody>();
                holdingObjectRB.useGravity = false;
                rbDrag = holdingObjectRB.drag;
            }
            else if(holdingObject.layer == LayerMask.NameToLayer("Hinge"))
            {
                isHoldingObject = true;
                setRigidbodyValues(holdingObject);
                initGrabDistance = Vector3.Distance(FirstPersonCamera.transform.position, holdingObject.transform.position);
                hingeJnt = holdingObject.GetComponent<HingeJoint>();
                springJoint = holdingObject.AddComponent<SpringJoint>();
                springJoint.autoConfigureConnectedAnchor = false;
                springJoint.spring = 40.0f;
                springJoint.damper = 5.0f;

                Vector3 posOnCircle = Vector3.Normalize(new Vector3(
                    holdingObject.transform.position.x - hingeJnt.connectedAnchor.x,
                    hingeJnt.connectedAnchor.y,
                    holdingObject.transform.position.z - hingeJnt.connectedAnchor.z
                    ));
                initHingeAngle = Vector3.Angle(new Vector3(1.0f, hingeJnt.connectedAnchor.y, 0.0f), posOnCircle);
                initHingeAngle = Vector2.SignedAngle(new Vector2(1.0f, 0.0f), new Vector2(posOnCircle.x, posOnCircle.z));
                initPlayerForward = new Vector2(FirstPersonCamera.transform.forward.x, FirstPersonCamera.transform.forward.z);

                springJoint.connectedAnchor = posOnCircle;
                springJoint.anchor = new Vector3(0.0f, 0.0f, 0.0f);

                //working for refrigerator doors
                //circleRadius = 0.8f * Vector2.Distance(
                //    new Vector2(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.z),
                //    new Vector2(raycastHit.point.x, raycastHit.point.z));
                //isHoldingObject = true;
                //setRigidbodyValues(holdingObject);
                //hingeJnt = holdingObject.GetComponent<HingeJoint>();


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
        //Debug.LogWarning("Moving object");
        Vector3 anchor;
        if (holdingObject.layer == LayerMask.NameToLayer("Moveable"))
        {
            
            holder.transform.position = FirstPersonCamera.transform.position + FirstPersonCamera.transform.forward * initGrabDistance;
            currentDistance = Vector3.Distance(holdingObject.transform.position, holder.transform.position);
            if(currentDistance > 0.9f)
            {
                dropObject();
                return;
            }
            if(currentDistance > 0.2f)
            {
                holdingObjectRB.drag = remap(0.0f, 0.9f, 1.0f, 5.0f, currentDistance);
                holdingObjectRB.AddForce(currentDistance * 100.0f * (holder.transform.position - holdingObject.transform.position));
            }
        }
        else if(holdingObject.layer == LayerMask.NameToLayer("Hinge"))
        {
            Debug.Log("circle radius: " + circleRadius);
            Vector2 playerForward = new Vector2(FirstPersonCamera.transform.forward.x, FirstPersonCamera.transform.forward.z);
            float playerAngle = Vector2.SignedAngle(playerForward, initPlayerForward);
            anchor = hingeJnt.connectedAnchor + new Vector3(Mathf.Cos(Mathf.Deg2Rad * (initHingeAngle + playerAngle)), hingeJnt.anchor.y, Mathf.Sin(Mathf.Deg2Rad * (initHingeAngle + playerAngle)));
            springJoint.connectedAnchor = anchor;
            Debug.Log("initHingeAngle: " + initHingeAngle + ", playerAngle: " + playerAngle);

            //springAnchor = FirstPersonCamera.transform.position + circleRadius * Vector3.Normalize(new Vector3(FirstPersonCamera.transform.forward.x, hingeJnt.connectedAnchor.y, FirstPersonCamera.transform.forward.z));
            //springJoint.connectedAnchor = springAnchor;


        }
    }
    private void dropObject()
    {
        if (holdingObject.layer == LayerMask.NameToLayer("Moveable"))
        {
            
            isHoldingObject = false;
            holdingObjectRB.drag = rbDrag;
            Vector3 force = (FirstPersonCamera.transform.position + FirstPersonCamera.transform.forward * initGrabDistance) - holder.transform.position;
            holdingObjectRB.AddForce(force * 5000.0f);
            holdingObjectRB.useGravity = true;
            holdingObject.transform.parent = oldParent;
            holdingObject = null;
            
            Debug.Log("Dropped object, added force: " + force.magnitude*100.0f);
            Destroy(holder);
        }
        else if (holdingObject.layer == LayerMask.NameToLayer("Hinge"))
        {
            Destroy(holdingObject.GetComponent<SpringJoint>());
            setOriginalRigidbodyValues(holdingObject);
            isHoldingObject = false;
            holdingObject = null;


        }
        //Debug.LogWarning("Released object");
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
    private float remap(float iMin, float iMax, float oMin, float oMax, float value)
    {
        float val;
        val = Mathf.InverseLerp(iMin, iMax, value);
        return Mathf.Lerp(oMin, oMax, val);
    }
}
