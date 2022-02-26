using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitItem : Item
{
    void Awake()
    {
        itemName = "Medkit";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void UseItem()
    {
        float health = GameManager.instance.health;
        Debug.Log("HEALTH: " + health);
        Debug.Log("Used medkit!");
        health += 50;
        if(health > 100.0f)
        {
            health = 100.0f;
        }
        GameManager.instance.health = health;
        Debug.Log("HEALTH: " + health);
    }
    public override string ToString()
    {
        return itemName + "item";
    }
}
