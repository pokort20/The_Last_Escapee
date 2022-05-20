/// Flashlight hover text class
/**
    This class handles the flashlight's hover
    text in the inventory.
*/
using UnityEngine;
using UnityEngine.EventSystems;

public class FlashlightHoverText : UIHoverText
{
    //Other variables
    private Inventory inventory;
    private GameManager gameManager;

    //Init
    void Start()
    {
        inventory = Inventory.instance;
        gameManager = GameManager.instance;
    }

    //Functions
    public override void OnPointerEnter(PointerEventData eventData)
    {
        float batteryPercentage = gameManager.batteryLevel / gameManager._maxBatteryLevel * 100.0f;
        inventory.itemInfoText = "FLASHLIGHT: " + Mathf.RoundToInt(batteryPercentage) + "%";
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        inventory.itemInfoText = null;
    }
}
