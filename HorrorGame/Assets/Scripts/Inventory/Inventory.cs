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
    //Variables
    public List<Item> items;
    public int inventorySize;
    private string interactTxt;
    private string infoTxt;
    private string itemInfoTxt;
    public string interactText
    {
        get { 
            return interactTxt; 
        }

        set {
            interactTxt = value;
            if (onInteractTextChangedCallback != null)
            {
                onInteractTextChangedCallback.Invoke();
            }
        }
    }
    public string infoText
    {
        get
        {
            return infoTxt;
        }

        set
        {
            infoTxt = value;
            if (onInfoTextChangedCallback != null)
            {
                onInfoTextChangedCallback.Invoke();
            }
        }
    }
    public string itemInfoText
    {
        get
        {
            return itemInfoTxt;
        }

        set
        {
            itemInfoTxt = value;
            if (onItemInfoTextChangedCallback != null)
            {
                onItemInfoTextChangedCallback.Invoke();
            }
        }
    }

    //Callbacks
    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;
    public delegate void OnInteractTextChanged();
    public OnInteractTextChanged onInteractTextChangedCallback;
    public delegate void OnInfoTextChanged();
    public OnInfoTextChanged onInfoTextChangedCallback;
    public delegate void OnItemInfoTextChanged();
    public OnItemInfoTextChanged onItemInfoTextChangedCallback;


    // Start is called before the first frame update
    void Start()
    {
        items = new List<Item>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool addToInventory(Item item)
    {
        if(items.Count < inventorySize)
        {
            items.Add(item);
            if (onInventoryChangedCallback != null)
            {
                onInventoryChangedCallback.Invoke();
            }
            printInventoryItems();
            return true;
        }
        else
        {
            Debug.Log("Inventory is full!");
            return false;
        }
    }
    public bool removeFromInventory(Item item)
    {
        items.Remove(item);
        if (onInventoryChangedCallback != null)
        {
            onInventoryChangedCallback.Invoke();
        }
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
    public List<Item> getItems()
    {
        return items;
    }
    private void printInventoryItems()
    {
        Debug.Log("INVENTORY:");
        foreach(Item item in items)
        {
            Debug.Log("           " + item.ToString());
        }
    }
    //private Item createItemObject(string itemName)
    //{
    //    Item retItem;
    //    switch(itemName)
    //    {
    //        case "Flashlight":
    //            retItem = new FlashlightItem();
    //            break;
    //        default:
    //            Debug.LogWarning("Crated default item!");
    //            retItem = new Item();
    //            break;
    //    }
    //    return retItem;
    //}
}
