using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    //public variables defined in UNITY
    public GameObject uiElements;
    public GameObject pauseMenuUI;
    public GameObject inventoryUI;
    public GameObject controlsUI;
    public GameObject deathScreen;
    public GameObject tutorialUI;

    public Transform itemSlotsParent;
    public Image flashlightImage;
    public Image batteryLevelImage;
    public Image healthFillImage;
    public TMP_Text interactText;
    public TMP_Text infoText;
    public TMP_Text itemInfoText;
    public Image interactImage;
    public Image lowHealthIndicator;
    public Image CursorCrosshair;
    public Texture2D cursorTexture;

    //other variables
    private bool isPaused;
    Canvas canvas;
    Inventory inventory;
    private float infoTextDefaultDuration = 5.0f;
    private float infoTextCurrentDuration;
    private GameManager gameManager;
    private Tutorial tutorial;
    private Color cursorCrosshairColor;

    //low health image indicator variables
    private Color lowHealthIndicatorColor;
    private float indicatorAlpha;
    private float alphaBase;
    private float alphaBaseOld;
    private float alphaDelta;

    InventorySlot[] itemSlots;
    private void Start()
    {
        isPaused = false;
        gameManager = GameManager.instance;
        inventory = Inventory.instance;
        tutorial = Tutorial.instance;

        inventory.onInventoryChangedCallback += updateInventoryUI;
        inventory.onInteractTextChangedCallback += updateInteractText;
        inventory.onInfoTextChangedCallback += displayInfoText;
        inventory.onItemInfoTextChangedCallback += updateItemInfoText;
        inventory.onMouseInteractionCallback += displayCursorCrosshair;
        gameManager.onPlayerDeathCallback += displayDeathScreen;
        itemSlots = itemSlotsParent.GetComponentsInChildren<InventorySlot>();
        flashlightImage.enabled = false;
        batteryLevelImage.enabled = false;
        lowHealthIndicatorColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        cursorCrosshairColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        alphaDelta = 0.0005f;
        alphaBaseOld = -1.0f;
        setCursorIcon();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        centerCursor();

        controlsUI.SetActive(false);
        inventoryUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        deathScreen.SetActive(false);
        uiElements.SetActive(true);
    }
    private void Update()
    {
        if(gameManager.isPlayerDead)
        {
            return;
        }
        updateInfoText();
        updateLowHealthIndicator();
        updateCursorCrosshair();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.LogWarning("Pressed ESC");
            if(isPaused)
            {
                if(controlsUI.activeInHierarchy)
                {
                    controlsUI.SetActive(false);
                    pauseMenuUI.SetActive(true);
                }
                else
                {
                    resumeGame();
                }
            }
            else
            {
                pauseGame();
            }
        }
        if(!isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Debug.Log("Opened inventory");
                //AudioManager.instance.playAudio("item_pickup");
                openInventory();
            }
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                Debug.Log("Closed inventory");
                //AudioManager.instance.playAudio("item_pickup");
                closeInventory();
            }
        }
    }
    private void setCursorIcon()
    {
        int textureSize = 48;
        Cursor.SetCursor(cursorTexture, new Vector2(textureSize / 2, textureSize / 2), CursorMode.ForceSoftware);
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
        inventory.inventoryOpened = true;
        CursorCrosshair.enabled = false;
        updateInventoryUI();
        Time.timeScale = 0.25f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if(tutorial != null)
        {
            tutorial.hideHelp("inventory");
        }
        tutorialUI.SetActive(false);
        inventoryUI.SetActive(true);
    }
    public void closeInventory()
    {
        inventory.inventoryOpened = false;
        CursorCrosshair.enabled = true;
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        tutorialUI.SetActive(true);
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
            //Debug.Log("Indicator alpha: " + indicatorAlpha);
            lowHealthIndicatorColor.a = indicatorAlpha;
            lowHealthIndicator.color = lowHealthIndicatorColor;
            lowHealthIndicator.enabled = true;
        }
        else
        {
            lowHealthIndicator.enabled = false;
        }
    }
    public void displayCursorCrosshair()
    {
        if (inventory.mouseInteracted == true)
        {
            cursorCrosshairColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            CursorCrosshair.enabled = true;
            CursorCrosshair.color = cursorCrosshairColor;
            if(tutorial != null)
            {
                tutorial.showHelp("moveObject");
            }
        }
    }
    public void updateCursorCrosshair()
    {
        if(CursorCrosshair.enabled)
        {
            cursorCrosshairColor = CursorCrosshair.color;
            cursorCrosshairColor.a -= 2.0f * Time.deltaTime;
            if(cursorCrosshairColor.a <= 0.0f)
            {
                CursorCrosshair.enabled = false;
            }
            else
            {
                CursorCrosshair.color = cursorCrosshairColor;
            }
        }
    }
    public void displayDeathScreen()
    {
        if(tutorial != null)
        {
            tutorial.hideAllHelps();
        }
        controlsUI.SetActive(false);
        inventoryUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        uiElements.SetActive(false);
        deathScreen.SetActive(true);

        centerCursor();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private float remap(float iMin, float iMax, float oMin, float oMax, float value)
    {
        float val;
        val = Mathf.InverseLerp(iMin, iMax, value);
        return Mathf.Lerp(oMin, oMax, val);
    }
    private void centerCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
    }
    //Pause Game Menu functions
    public void pauseGame()
    {
        setCursorIcon();
        closeInventory();
        isPaused = true;
        centerCursor();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        uiElements.SetActive(false);
        tutorialUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        gameManager.pauseGame();
    }
    public void resumeGame()
    {
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        tutorialUI.SetActive(true);
        uiElements.SetActive(true);

        gameManager.unPauseGame();
    }
    public void OnResumeButtonUse()
    {
        Debug.Log("Resuming game!");
        gameManager.setTimeScale(1.0f);
        AudioManager.instance.playAudio("menu_click");
        gameManager.setTimeScale(0.0f);
        resumeGame();
    }
    public void OnControlsButtonUse()
    {
        Debug.Log("Opened controls!");
        gameManager.setTimeScale(1.0f);
        AudioManager.instance.playAudio("menu_click");
        gameManager.setTimeScale(0.0f);
        pauseMenuUI.SetActive(false);
        controlsUI.SetActive(true);
    }
    public void OnQuitButtonUse()
    {
        Debug.Log("Returning to main menu!");
        gameManager.setTimeScale(1.0f);
        AudioManager.instance.playAudio("menu_click");
        gameManager.setTimeScale(0.0f);
        SceneTransitionData std = FindObjectOfType<SceneTransitionData>();
        if(std != null)
        {
            Destroy(std.gameObject);
        }
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
    }
    public void OnBackButtonUse()
    {
        Debug.Log("Returning back to pause menu!");
        gameManager.setTimeScale(1.0f);
        AudioManager.instance.playAudio("menu_click");
        gameManager.setTimeScale(0.0f);
        controlsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
    public void OnRetryButtonUse()
    {
        Debug.Log("Retrying");
        gameManager.setTimeScale(1.0f);
        AudioManager.instance.playAudio("menu_click");
        gameManager.setTimeScale(0.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
