/// Health hover text class
/**
    This class handles the health hover
    text in the inventory.
*/
using UnityEngine;
using UnityEngine.EventSystems;

public class HealthHoverText : UIHoverText
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
        inventory.itemInfoText = "HEALTH: " + Mathf.RoundToInt(gameManager.health); ;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        inventory.itemInfoText = null;
    }
}
