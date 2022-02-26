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
        useButton.interactable = true;
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
        if(item.oneTimeUse)
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
        
    }
}
