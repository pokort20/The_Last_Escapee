using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI :MonoBehaviour
{
    public GameObject inventoryUI;
    List<Item> items;
    Canvas canvas;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Opened/Closed inventory");
            toggleInventory();
        }
    }
    public InventoryUI(List<Item> Items)
    {
        items = Items;
        initUI();
    }
    private void initUI()
    {
        canvas = new Canvas();

    }
    public void toggleInventory()
    {
        //Time.timeScale = 0.1f;
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }
}
