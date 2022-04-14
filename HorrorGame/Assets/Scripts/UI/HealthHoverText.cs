using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealthHoverText : UIHoverText
{
    private Inventory inventory;
    private GameManager gameManager;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        inventory.itemInfoText = "HEALTH: " + Mathf.RoundToInt(gameManager.health); ;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        inventory.itemInfoText = null;
    }
    void Start()
    {
        inventory = Inventory.instance;
        gameManager = GameManager.instance;
    }
}
