using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image image;
    public Item item;
    public Button useButton;
    public TMP_Text textObject;

    public void FillInventorySlot(Item newItem)
    {
        Debug.Log("Filled inventory slot " + name + " with " + newItem.name);
        item = newItem;
        image.sprite = item.icon;
        image.enabled = true;
        //useButton.interactable = true;
        textObject.text = item.itemName;
    }
    public void ClearInventorySlot()
    {
        item = null;
        image.sprite = null;
        image.enabled = false;
        useButton.interactable = false;
        textObject.text = "";
    }
    public void OnUseButton()
    {
        Debug.Log("Clicked useButton");
        item.UseItem();
        //Item temp = item;
        if(item.isUseable)
        {
            Inventory.instance.removeFromInventory(item);
        }
    }
    private void Awake()
    {
        Debug.Log("Inventory slot " + ToString() + " started");
        useButton.interactable = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(item != null && useButton != null)
        {
            if(item.GetType() != typeof(BatteryItem) && item.GetType() != typeof(MedkitItem) && item.GetType() != typeof(FlashlightItem))
            {
                item.isUseable = true;
            }
            else
            {
                if (item.GetType() == typeof(BatteryItem) && GameManager.instance.batteryLevel < GameManager.instance._maxBatteryLevel / 2)
                {
                    item.isUseable = true;
                }
                else if(item.GetType() == typeof(MedkitItem) && GameManager.instance.health < GameManager.instance._maxHealth)
                {
                    item.isUseable = true;
                }
                else
                {
                    item.isUseable = false;
                }
            }



            if(item.isUseable)
            {
                useButton.interactable = true;
            }
            else
            {
                useButton.interactable = false;
            }
        }
    }
}
