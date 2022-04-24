using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Variables
    public static GameManager instance;
    public float _movementSpeed;
    public float _maxHealth;
    public float _batterLevel;
    public float _maxBatteryLevel;
    public float _maxStamina;
    public bool _flashlightEnabled;
    private bool _isPaused;
    void Awake()
    {
        Debug.Log("Started GameManager");
        if (instance != null)
        {
            Debug.LogError("Multiple instances of GameManager!!!");
            return;
        }
        instance = this;

        movementSpeed = _movementSpeed;
        health = _maxHealth*0.51f;
        batteryLevel = _batterLevel;
        stamina = _maxStamina;
        flashlightEnabled = _flashlightEnabled;
        isPaused = false;
    }
    void Start()
    {
        printGameVariables();
        loadSceneData();
    }
    //Properties
    public float health { get; set; }
    public float movementSpeed { get; set; }
    public float batteryLevel { get; set; }
    public float stamina { get; set; }
    public bool flashlightEnabled { get; set; }
    public bool isPaused 
    {
        get
        {
            return _isPaused;
        }
        set
        {
            _isPaused = value;
            if(onGamePausedCallback != null)
            {
                onGamePausedCallback.Invoke();
            }
        }
    }
    //Callbacks
    public delegate void OnGamePaused();
    public OnGamePaused onGamePausedCallback;

    void Update()
    {

    }
    private void loadSceneData()
    {
        SceneTransitionData std = FindObjectOfType<SceneTransitionData>();
        if(std != null)
        {
            instance.health = std.health;
            instance.stamina = std.stamina;
            instance.batteryLevel = std.batteryLevel;
            instance.flashlightEnabled = std.flashlightEnabled;

            Debug.Log("Transitioned health: " + std.health);
            Debug.Log("Transitioned stamina: " + std.stamina);
            Debug.Log("Transitioned battery level: " + std.batteryLevel);
            Debug.Log("Transitioned flashlight enabled: " + std.flashlightEnabled);
            foreach(Item item in std.inventoryItems)
            {
                Inventory.instance.addToInventory(item);

                Debug.Log("Transitioned item: " + item.itemName);
            }
            Destroy(std.gameObject);
        }
        else
        {
            Debug.LogWarning("Can not find Scene transition data, starting with default stats and items");
        }
    }
    public void printGameVariables()
    {
        Debug.Log("GAME VARIABLES");
        Debug.Log("            Health: " + health);
        Debug.Log("            Movement speed: " + movementSpeed);
        Debug.Log("            batteryLevel: " + batteryLevel);
        Debug.Log("            stamina: " + stamina);
    }

}
