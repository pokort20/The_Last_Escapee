using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
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
    public void addToInventory(Item item)
    {
        items.Add(item);
        printInventoryItems();
    }
    public bool removeFromInventory(Item item)
    {
        printInventoryItems();
        return true;
    }
    private void printInventoryItems()
    {
        Debug.Log("INVENTORY:");
        foreach(Item item in items)
        {
            Debug.Log("   " + item.ToString());
        }
    }
}
