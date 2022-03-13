using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : Item
{
    void Awake()
    {
        isUseable = false;
        itemName = "Key";
    }
    public override void UseItem()
    {
        Debug.Log("Used key!");
    }
    public override string ToString()
    {
        return itemName + "item";
    }
}
