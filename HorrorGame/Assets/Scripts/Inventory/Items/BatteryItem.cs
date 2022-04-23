using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryItem : Item
{
    void Awake()
    {
        itemName = "Batteries";
    }

    // Update is called once per frame
    void Update()
    {
        //if(GameManager.instance.batteryLevel < GameManager.instance._maxBatteryLevel / 2)
        //{
        //    Debug.Log("TRUE: " + GameManager.instance.batteryLevel);
        //    isUseable = true;
        //}
        //else
        //{
        //    Debug.Log("FALSE: " + GameManager.instance.batteryLevel);
        //    isUseable = false;
        //}
    }
    public override void UseItem()
    {
        base.UseItem();
        Debug.Log("Battery level: " + GameManager.instance.batteryLevel);
        Debug.Log("Used battery!");
        GameManager.instance.batteryLevel += GameManager.instance._maxBatteryLevel*0.5f;
        Debug.Log("Battery level: " + GameManager.instance.batteryLevel);
    }
    public override string ToString()
    {
        return itemName + "item";
    }
}
