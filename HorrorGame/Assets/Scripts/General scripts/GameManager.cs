using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private float _health;
    private bool _isPaused;
    private bool _isPlayerDead;

    //Init
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
        isPlayerDead = false;
    }
    void Start()
    {
        printGameVariables();
        loadSceneData();
        setTimeScale(1.0f);
    }

    //Properties
    public float health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            if(_health <= 0.0f)
            {
                isPlayerDead = true;
                setTimeScale(0.0f);
            }
        }
    }
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
    public bool isPlayerDead
    {
        get
        {
            return _isPlayerDead;
        }
        set
        {
            _isPlayerDead = value;
            if(onPlayerDeathCallback != null)
            {
                if(_isPlayerDead)onPlayerDeathCallback.Invoke();
            }
        }
    }
    
    //Callbacks
    public delegate void OnGamePaused();
    public OnGamePaused onGamePausedCallback;
    public delegate void OnPlayerDeath();
    public OnPlayerDeath onPlayerDeathCallback;

    //Functions
    private void loadSceneData()
    {
        //Main menu scene does not require any loaded scene data
        if (SceneManager.GetActiveScene().name == "MainMenuScene")
        {
            return;
        }

        //First check for save data transitioned from previous scene
        SceneTransitionData std = FindObjectOfType<SceneTransitionData>();

        //if not present, try loading it from save file
        if (std == null)
        {
            std = SaveLoadSystem.LoadGame(SceneManager.GetActiveScene().name);
        }

        if (std != null)
        {
            //set game variables
            instance.health = std.health;
            instance.stamina = std.stamina;
            instance.batteryLevel = std.batteryLevel;
            instance.flashlightEnabled = std.flashlightEnabled;

            //set inventory items
            foreach(Item item in std.inventoryItems)
            {
                Inventory.instance.addToInventory(item);
            }
        }
        else
        {
            Debug.LogWarning("Can not find scene transition nor load game, starting with default stats and items");
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
    public void pauseGame()
    {
        instance.isPaused = true;
        Time.timeScale = 0.0f;
    }
    public void unPauseGame()
    {
        instance.isPaused = false;
        Time.timeScale = 1.0f;
    }
    public void setTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

}
