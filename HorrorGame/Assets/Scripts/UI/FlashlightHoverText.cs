using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FlashlightHoverText : UIHoverText
{
    private Inventory inventory;
    private GameManager gameManager;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        float batteryPercentage = gameManager.batteryLevel / gameManager._maxBatteryLevel * 100.0f;
        inventory.itemInfoText = "FLASHLIGHT: " + Mathf.RoundToInt(batteryPercentage) + "%";
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        inventory.itemInfoText = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        gameManager = GameManager.instance;
    }
}
