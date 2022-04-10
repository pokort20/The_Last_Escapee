using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    //public variables defined in UNITY
    public GameObject inventoryUI;
    public Transform itemSlotsParent;
    public Image flashlightImage;
    public Image batteryLevelImage;
    public Image healthFillImage;
    public TMP_Text interactText;
    public TMP_Text infoText;
    public TMP_Text itemInfoText;
    public Image interactImage;
    public Image lowHealthIndicator;

    //other variables
    Canvas canvas;
    Inventory inventory;
    private float infoTextDefaultDuration = 5.0f;
    private float infoTextCurrentDuration;
    private GameManager gameManager;

    //low health image indicator variables
    private Color lowHealthIndicatorColor;
    private float indicatorAlpha;
    private float alphaBase;
    private float alphaBaseOld;
    private float alphaDelta;

    InventorySlot[] itemSlots;
    private void Start()
    {
        inventory = Inventory.instance;
        inventory.onInventoryChangedCallback += updateInventoryUI;
        inventory.onInteractTextChangedCallback += updateInteractText;
        inventory.onInfoTextChangedCallback += displayInfoText;
        inventory.onItemInfoTextChangedCallback += updateItemInfoText;
        itemSlots = itemSlotsParent.GetComponentsInChildren<InventorySlot>();
        flashlightImage.enabled = false;
        batteryLevelImage.enabled = false;
        gameManager = GameManager.instance;
        lowHealthIndicatorColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        alphaDelta = 0.0005f;
        alphaBaseOld = -1.0f;
    }
    private void Update()
    {
        updateInfoText();
        updateLowHealthIndicator();
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
        Debug.Log("Items.length: " + itemSlots.Length);
        Debug.Log("Inventory.items.count: " + inventory.items.Count);

        float fillAmount = GameManager.instance.health / GameManager.instance._maxHealth;
        healthFillImage.fillAmount = fillAmount;
        if(inventory.hasItem(typeof(FlashlightItem)) > 0)
        {
            flashlightImage.enabled = true;
            batteryLevelImage.enabled = true;
            fillAmount = GameManager.instance.batteryLevel / GameManager.instance._maxBatteryLevel;
            Debug.Log("batteryLevel: " + GameManager.instance.batteryLevel + " maxBatteryLevel: " + GameManager.instance._maxBatteryLevel + " fill amount: " + fillAmount);
            if (fillAmount > 1.0f)
            {
                fillAmount = 1.0f;
            }
            batteryLevelImage.fillAmount = fillAmount;
        }

        for(int i = 0; i < itemSlots.Length; ++i)
        {
            //Debug.Log("Item slot: " + i);
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
    public void updateInteractText()
    {

        if (inventory.interactText != null)
        {
            Debug.Log("Enabling interact text");
            interactText.text = inventory.interactText;
            interactText.enabled = true;
            interactImage.enabled = true;
        }
        else
        {
            Debug.Log("Disabling interact text");
            interactText.enabled = false;
            interactImage.enabled = false;
        }
    }
    public void updateInfoText()
    {
        infoTextCurrentDuration -= Time.deltaTime;
        if(infoTextCurrentDuration < 0.0f)
        {
            infoText.enabled = false;
        }
    }
    public void updateItemInfoText()
    {
        if(inventory.itemInfoText != null)
        {
            itemInfoText.text = inventory.itemInfoText;
            itemInfoText.enabled = true;
        }
        else
        {
            itemInfoText.enabled = false;
        }
    }
    public void displayInfoText()
    {
        infoText.text = inventory.infoText;
        infoTextCurrentDuration = infoTextDefaultDuration;
        infoText.enabled = true;
    }
    public void openInventory()
    {
        updateInventoryUI();
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
    public void updateLowHealthIndicator()
    {

        if(gameManager.health <= gameManager._maxHealth * 0.5f)
        {
            alphaBase = 0.11f - remap(0.0f, gameManager._maxHealth * 0.5f, 0.0f, 0.10f, gameManager.health);
            if (alphaBase != alphaBaseOld)
            {
                indicatorAlpha = alphaBase;
                alphaBaseOld = alphaBase;
            }
            else
            {
                indicatorAlpha += alphaDelta;
            }
            if(indicatorAlpha > alphaBase + 0.02f || indicatorAlpha < alphaBase - 0.02f)
            {
                alphaDelta = -alphaDelta;
            }
            Debug.Log("Indicator alpha: " + indicatorAlpha);
            lowHealthIndicatorColor.a = indicatorAlpha;
            lowHealthIndicator.color = lowHealthIndicatorColor;
            lowHealthIndicator.enabled = true;
        }
        else
        {
            lowHealthIndicator.enabled = false;
        }
    }
    private float remap(float iMin, float iMax, float oMin, float oMax, float value)
    {
        float val;
        val = Mathf.InverseLerp(iMin, iMax, value);
        return Mathf.Lerp(oMin, oMax, val);
    }
}
