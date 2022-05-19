using UnityEngine;

public class PickupObject : MonoBehaviour
{
    //values set in inspector
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
    private Vector3 holdingPosition;

    private Inventory inventory;
    private float initGrabDistance;
    private float currentGrabDistance;
    RaycastHit raycastHit;
    private bool isHoldingObject;
    GameObject holdingObject;
    private SpringJoint springJoint;
    private HingeJoint hingeJnt;

    //old RB values
    bool rbUseGravity;
    float rbDrag;
    float rbAngularDrag;

    void Start()
    {
        Debug.Log("Started PickupObjects");
        inventory = Inventory.instance;
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
        if(isHoldingObject)
        {
            if (Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out raycastHit, maxGrabDistance*2.0f))
            {
                if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Moveable") || raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Hinge"))
                {
                    colided = true;
                }
            }
        }
        else
        {
            if (Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out raycastHit, maxGrabDistance))
            {
                if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Moveable") || raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Hinge"))
                {
                    colided = true;
                }
            }
        }

        if (!isHoldingObject)
        {
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
            if(holdingObject.layer == LayerMask.NameToLayer("Moveable") && (raycastHit.collider == null || raycastHit.collider.gameObject != holdingObject))
            {
                if (raycastHit.collider == null)
                {
                    Debug.Log("dropping coz raycast not hit");
                }
                else
                {
                    Debug.Log("Dropping coz raycast hit different object");
                }
                dropObject();
                return;
            }
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

    private void pickupObject()
    {
        //Debug.Log("Raycast collided with: " + raycastHit.collider.gameObject.name);
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
                initGrabDistance = Vector2.Distance(new Vector2(FirstPersonCamera.transform.position.x,
                    FirstPersonCamera.transform.position.z), new Vector2(raycastHit.point.x, raycastHit.point.z));
                holdingObject.AddComponent<SpringJoint>();
                springJoint = holdingObject.GetComponent<SpringJoint>();
                hingeJnt = holdingObject.GetComponent<HingeJoint>();
                hingeJnt.useSpring = true;
                springJoint.spring = 80.0f;
                springJoint.damper = 1.0f;
                springJoint.tolerance = 0.0f;
                springJoint.autoConfigureConnectedAnchor = false;
                springJoint.anchor = springJoint.transform.InverseTransformPoint(new Vector3(raycastHit.point.x, FirstPersonCamera.transform.position.y, raycastHit.point.z));
                holdingPosition = new Vector3(raycastHit.point.x, FirstPersonCamera.transform.position.y, raycastHit.point.z);
                springJoint.connectedAnchor = holdingPosition;
                
            }

        }
    }
    private void moveObject()
    {
        //Debug.LogWarning("Moving object");
        if (holdingObject.layer == LayerMask.NameToLayer("Moveable"))
        {
            
            holder.transform.position = FirstPersonCamera.transform.position + FirstPersonCamera.transform.forward * initGrabDistance;
            currentGrabDistance = Vector3.Distance(holdingObject.transform.position, holder.transform.position);
            if(currentGrabDistance > 0.9f)
            {
                Debug.Log("dropping coz dist small");
                dropObject();
                return;
            }
            if(currentGrabDistance > 0.2f)
            {
                holdingObjectRB.drag = remap(0.0f, 0.9f, 1.0f, 5.0f, currentGrabDistance);
                holdingObjectRB.AddForce(currentGrabDistance * 100.0f * (holder.transform.position - holdingObject.transform.position));
            }
        }
        else if(holdingObject.layer == LayerMask.NameToLayer("Hinge"))
        {
            if (Vector2.Distance(new Vector2(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.z), new Vector2(holdingObject.transform.position.x, holdingObject.transform.position.z)) > maxGrabDistance)
            {
                dropObject();
                return;
            }
            if(holdingObject.GetComponent<Rigidbody>().velocity.magnitude == 0.0f)
            {
                holdingObject.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-0.0001f, 0.0001f), 0, Random.Range(-0.0001f, 0.0001f));
            }
            Vector2 xzPos = new Vector2(FirstPersonCamera.transform.forward.x, FirstPersonCamera.transform.forward.z);
            xzPos.Normalize();
            xzPos *= initGrabDistance;
            xzPos += new Vector2(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.z);
            holdingPosition = new Vector3(xzPos.x, FirstPersonCamera.transform.position.y, xzPos.y);
            springJoint.connectedAnchor = holdingPosition;
        }
    }
    private void dropObject()
    {
        if (holdingObject.layer == LayerMask.NameToLayer("Moveable"))
        {
            
            isHoldingObject = false;
            holdingObjectRB.drag = rbDrag;
            //Vector3 force = (FirstPersonCamera.transform.position + FirstPersonCamera.transform.forward * initGrabDistance) - holder.transform.position;
            //holdingObjectRB.AddForce(force * 250000.0f * Time.deltaTime);
            holdingObjectRB.useGravity = true;
            holdingObject.transform.parent = oldParent;
            holdingObject = null;
            
            //Debug.Log("Dropped object, added force: " + force.magnitude*100.0f);
            Destroy(holder);
        }
        else if (holdingObject.layer == LayerMask.NameToLayer("Hinge"))
        {
            hingeJnt.useSpring = false;
            Destroy(springJoint);
            springJoint = null;
            isHoldingObject = false;
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
