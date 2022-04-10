using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : Interactable
{
    public bool isLocked;
    public Transform door;
    public string infoText;


    private Inventory inventory;

    Rigidbody rb;
    void Awake()
    {
        inventory = Inventory.instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        canInteract = true;
        rb = door.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning(door.name + "has no rigidbody component!");
        }
        if (isLocked)
        {
            rb.isKinematic = true;
        }
        else
        {
            GetComponent<BoxCollider>().enabled = false;
            transform.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    // Update is called once per frame
    void Update()
    {
           
    }
    public override void Interact()
    {
        if(inventory.hasItem(typeof(KeyItem)) > 0)
        {
            //UNLOCK DOOR
            if (rb == null)
            {
                Debug.LogWarning(door.name + "has no rigidbody component!");
                return;
            }
            foreach(Item item in inventory.getItems())
            {
                if(typeof(KeyItem) == item.GetType())
                {
                    inventory.removeFromInventory(item);
                    break;
                }
            }
            GetComponent<BoxCollider>().enabled = false;
            rb.isKinematic = false;
            transform.gameObject.layer = LayerMask.NameToLayer("Default");
            isLocked = false;
        }
        else
        {
            if (infoText != null)
            {
                inventory.infoText = infoText;
            }
            Debug.Log("Player does not have any key!");
        }
    }
    public override string InteractText()
    {
        return "Unlock";
    }

}
