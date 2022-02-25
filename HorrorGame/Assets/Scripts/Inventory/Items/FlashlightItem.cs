using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlashlightItem : Item
{
    void Awake()
    {
        oneTimeUse = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public override void UseItem()
    {
        Debug.Log("Used flashlight!");
    }
    public override string ToString()
    {
        return itemName + "item";
    }
}
