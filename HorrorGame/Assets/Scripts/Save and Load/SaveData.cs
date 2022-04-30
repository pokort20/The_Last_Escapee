using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float health;
    public float stamina;
    public float batteryLevel;
    public bool flashlightEnabled;
    public string[] items;
    public SaveData(SceneTransitionData sceneData)
    {
        health = sceneData.health;
        stamina = sceneData.stamina;
        batteryLevel = sceneData.batteryLevel;
        flashlightEnabled = sceneData.flashlightEnabled;
        int len = sceneData.inventoryItems.Count();
        items = new string[len];
        for(int i = 0; i <len; ++i)
        {
            items[i] = sceneData.inventoryItems[i].itemName;
        }
    }
}
