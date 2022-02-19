using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    void Awake()
    {
        Debug.Log("Started Inventory");
        if(instance != null)
        {
            Debug.LogError("Multiple instances of invenotory!!!");
            return;
        }
        instance = this;
    }
    private List<Item> items;

    // Start is called before the first frame update
    void Start()
    {
        items = new List<Item>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void addToInventory(string itemName)
    {
        items.Add(createItemObject(itemName));
        printInventoryItems();
    }
    public bool removeFromInventory(Item item)
    {
        printInventoryItems();
        return true;
    }
    public int hasItem(Type type)
    {
        int retval = 0;
        foreach(Item item in items)
        {
            if(item.GetType() == type)
            {
                retval++;
            }
        }
        return retval;
    }
    private void printInventoryItems()
    {
        Debug.Log("INVENTORY:");
        foreach(Item item in items)
        {
            Debug.Log("           " + item.ToString());
        }
    }
    private Item createItemObject(string itemName)
    {
        Item retItem;
        switch(itemName)
        {
            case "Flashlight":
                retItem = new FlashlightItem();
                break;
            default:
                Debug.LogWarning("Crated default item!");
                retItem = new Item();
                break;
        }
        return retItem;
    }
}
