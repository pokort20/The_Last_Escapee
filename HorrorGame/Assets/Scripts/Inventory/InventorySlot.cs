using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public Image image;
    public Item item;
    public Button useButton;

    public void FillInventorySlot(Item newItem)
    {
        Debug.Log("Filled inventory slot");
        item = newItem;
        image.sprite = item.icon;
        image.enabled = true;
        useButton.interactable = true;
    }
    public void ClearInventorySlot()
    {
        item = null;
        image.sprite = null;
        image.enabled = false;
        useButton.interactable = false;
    }
    public void OnUseButton()
    {
        Debug.Log("Clicked useButton");
        item.UseItem();
        if(item.oneTimeUse)
        {
            Inventory.instance.removeFromInventory(item);
            ClearInventorySlot();
        }
    }
    private void Awake()
    {
        Debug.Log("Inventory slot " + this.ToString() + " started");
        useButton.interactable = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
