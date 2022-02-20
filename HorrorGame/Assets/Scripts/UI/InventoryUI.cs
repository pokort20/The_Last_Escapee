using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI :MonoBehaviour
{
    public GameObject inventoryUI;
    public Transform itemSlotsParent;
    Canvas canvas;
    Inventory inventory;

    InventorySlot[] itemSlots;
    private void Start()
    {
        inventory = Inventory.instance;
        inventory.onInventoryChangedCallback += updateInventoryUI;
        itemSlots = itemSlotsParent.GetComponentsInChildren<InventorySlot>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Opened inventory");
            openInventory();
        }
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            Debug.Log("Closed inventory"); 
            closeInventory();
        }
    }
    public void updateInventoryUI()
    {
        Debug.Log("Updating inventory UI!");
        for(int i = 0; i < itemSlots.Length; ++i)
        {
            Debug.Log("Item slot: " + i);
            if(i < inventory.items.Count)
            {
                itemSlots[i].FillInventorySlot(inventory.items[i]);
            }
            else
            {
                itemSlots[i].ClearInventorySlot();
            }
        }

    }
    public void openInventory()
    {
        Time.timeScale = 0.25f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        inventoryUI.SetActive(true);
    }
    public void closeInventory()
    {
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        inventoryUI.SetActive(false);
    }
}
