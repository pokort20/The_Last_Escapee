using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindInteraction : MonoBehaviour
{
    public Camera FirstPersonCamera;
    public float maxPickupDistance;

    RaycastHit raycastHit;
    GameObject itemObject;
    GameObject interactingObject;
    Inventory inventory;
    private bool pressedKey;
    private LayerMask ignoreMask;
    private bool isInteracting = false;
    void Start()
    {
        inventory = Inventory.instance;
        pressedKey = false;
        ignoreMask = LayerMask.GetMask("Player");
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
        if (Physics.Raycast(origin: FirstPersonCamera.transform.position, direction: FirstPersonCamera.transform.forward, hitInfo: out raycastHit, maxDistance: maxPickupDistance, layerMask: ~ignoreMask))
        {
            //Debug.Log("Collided with: " + raycastHit.collider.gameObject.name);
            //Debug.Log("        layer: " + LayerMask.LayerToName(raycastHit.collider.gameObject.layer));

            //ITEMS
            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                handleInteractionText("E - pick up");
                if (itemObject!=null)
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
            //INTERACTABLES
            else if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                handleInteractionText("E - Interact");
                interactingObject = raycastHit.collider.gameObject;
                //Debug.Log("Hit interactable object!" + raycastHit.collider.gameObject.ToString());
                if (pressedKey)
                {
                    interactingObject.GetComponent<Interactable>().Interact();
                    //Debug.Log("Interact!");
                    pressedKey = false;
                }
            }
            //not item nor interactable
            else
            {
                handleInteractionText(null);
                if (itemObject != null) removeItemHighlight(itemObject);
            }
        }
        else
        {
            handleInteractionText(null);
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
    private void handleInteractionText(string interactionText)
    {
        if (isInteracting)
        {
            inventory.interactText = interactionText;
            isInteracting = false;
        }
        else
        {
            inventory.interactText = interactionText;
            isInteracting = true;
        }
    }
}
