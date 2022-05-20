/// Inventory slot class
/**
    This class handles the inventory slot logic
    as well as its UI.
*/
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Public variables defined in Unity inspector
    public Image image;
    public Item item;
    public Button useButton;
    public TMP_Text textObject;

    //Other variables
    private Inventory inventory;

    //Functions
    public void FillInventorySlot(Item newItem)
    {
        item = newItem;
        image.sprite = item.icon;
        image.enabled = true;
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
        item.UseItem();
        if(item.isUseable)
        {
            inventory.removeFromInventory(item);
            inventory.itemInfoText = null;
        }
    }

    //Init
    void Awake()
    {
        Debug.Log("Inventory slot " + ToString() + " started");
        useButton.interactable = false;

    }
    void Start()
    {
        inventory = Inventory.instance;
    }

    //Update
    void Update()
    {

        if(item != null && useButton != null)
        {
            if(item.GetType() != typeof(BatteryItem) && item.GetType() != typeof(MedkitItem) && item.GetType() != typeof(FlashlightItem) && item.GetType() != typeof(KeyItem) && item.GetType() != typeof(SecurityCardItem))
            {
                item.isUseable = true;
            }
            else
            {
                if (item.GetType() == typeof(BatteryItem) && GameManager.instance.batteryLevel <= GameManager.instance._maxBatteryLevel / 2)
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null && item.itemInfo != null)
        {
            inventory.itemInfoText = item.itemInfo;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventory.itemInfoText = null;
    }
}
