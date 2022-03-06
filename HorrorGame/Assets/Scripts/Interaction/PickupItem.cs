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
    private bool pressedKey;
    private LayerMask ignoreMask;
    void Start()
    {
        inventory = Inventory.instance;
        pressedKey = false;
        ignoreMask = LayerMask.NameToLayer("Player");
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            pressedKey = true;
        }
        if(Input.GetKeyUp(KeyCode.E))
        {
            pressedKey = false;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(FirstPersonCamera.transform.position, Vector3.Normalize(FirstPersonCamera.transform.forward)*maxPickupDistance, Color.blue, 0.0f);
        if (Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out raycastHit, maxPickupDistance, ~ignoreMask))
        {
            Debug.Log("Collided with: " + raycastHit.collider.gameObject.name);
            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                if(itemObject!=null)
                {
                    if(!ReferenceEquals(itemObject, raycastHit.collider.gameObject))
                    {
                        removeItemHighlight(itemObject);
                    }
                }
                itemObject = raycastHit.collider.gameObject;
                highlightItem(itemObject);
                //Debug.Log("Coliding with item" + itemObject.name);
                if (pressedKey)
                {
                    if (inventory.addToInventory(itemObject.GetComponent<Item>()))
                    {
                        Debug.Log("Picked up " + itemObject.name + " and added it to inventory!");
                        itemObject.SetActive(false);
                    }
                }
            }
            else
            {
                if(itemObject != null)removeItemHighlight(itemObject);
            }
        }
        else
        {
            if (itemObject != null) removeItemHighlight(itemObject);
        }
    }
    private void highlightItem(GameObject gameObject)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            gameObject.GetComponent<Renderer>().material.SetFloat("_EmissionLevel", 1.0f);
        }
        foreach(Renderer ren in gameObject.GetComponentsInChildren<Renderer>())
        {
            if(ren != null)
            {
                ren.material.SetFloat("_EmissionLevel", 1.0f);
            }
        }
    }
    private void removeItemHighlight(GameObject highlightedObject)
    {
        if(highlightedObject != null)
        {
            Renderer renderer = highlightedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                highlightedObject.GetComponent<Renderer>().material.SetFloat("_EmissionLevel", 0.0f);
            }
            foreach (Renderer ren in highlightedObject.GetComponentsInChildren<Renderer>())
            {
                if (ren != null)
                {
                    ren.material.SetFloat("_EmissionLevel", 0.0f);
                }
            }
        }
        else
        {
            Debug.LogWarning("Attempted to remove highlight from null object");
        }
    }
}
