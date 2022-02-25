using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float _movementSpeed;
    public float _health;
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
    }

    public float health { get; set; }
    public float movementSpeed { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        printGameVariables();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void printGameVariables()
    {
        Debug.Log("GAME VARIABLES");
        Debug.Log("            Health: " + health);
        Debug.Log("            Movement speed: " + movementSpeed);
    }

}
