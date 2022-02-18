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
        if (Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out raycastHit, maxPickupDistance))
        {
            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                itemObject = raycastHit.collider.gameObject;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    GetComponentInParent<Inventory>().addToInventory(itemObject.GetComponent<Item>());
                    Debug.Log("Picked up " + itemObject.GetComponent<Item>().ToString() + " and added it to inventory!");
                    itemObject.SetActive(false);

                }
            }
        }
    }
}
