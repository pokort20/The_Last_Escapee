using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float _movementSpeed;
    public float _health;
    public float _batterLevel;
    public float _stamina;
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
        health = _health;
        batteryLevel = _batterLevel;
        stamina = _stamina;
    }

    public float health { get; set; }
    public float movementSpeed { get; set; }
    public float batteryLevel { get; set; }
    public float stamina { get; set; }


    void Start()
    {
        printGameVariables();
    }

    void Update()
    {
        
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
