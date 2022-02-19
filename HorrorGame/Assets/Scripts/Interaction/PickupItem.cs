using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Camera FirstPersonCamera;
    public float maxPickupDistance;

    RaycastHit raycastHit;
    GameObject itemObject;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.DrawRay(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, Color.green, 0.0f);
        if (Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out raycastHit, maxPickupDistance))
        {
            //Debug.Log("Collided with layer: " + raycastHit.collider.gameObject.ToString());
            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                itemObject = raycastHit.collider.gameObject;
                //Debug.Log("Coliding with item" + itemObject.tag);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Inventory inventory = Inventory.instance;
                    inventory.addToInventory(itemObject.tag);
                    Debug.Log("Picked up " + itemObject.tag + " and added it to inventory!");
                    Destroy(itemObject);

                }
            }
        }
    }
}
