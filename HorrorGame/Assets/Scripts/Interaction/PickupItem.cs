using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Camera FirstPersonCamera;
    public float maxPickupDistance;

    RaycastHit raycastHit;
    GameObject itemObject;
    Inventory inventory;
    void Start()
    {
        inventory = Inventory.instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(FirstPersonCamera.transform.position, Vector3.Normalize(FirstPersonCamera.transform.forward)*maxPickupDistance, Color.green, 0.0f);
        if (Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out raycastHit, maxPickupDistance))
        {
            //Debug.Log("Collided with layer: " + raycastHit.collider.gameObject.ToString());
            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                itemObject = raycastHit.collider.gameObject;
                //Debug.Log("Coliding with item" + itemObject.name);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (inventory.addToInventory(itemObject.GetComponent<Item>()))
                    {
                        Debug.Log("Picked up " + itemObject.name + " and added it to inventory!");
                        itemObject.SetActive(false);
                    }
                }
            }
        }
    }
}
