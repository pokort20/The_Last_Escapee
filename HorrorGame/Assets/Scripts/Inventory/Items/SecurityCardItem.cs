using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCardItem : Item
{
    // Start is called before the first frame update
    void Awake()
    {
        isUseable = false;
        itemName = "labCard";
    }
    public override void UseItem()
    {
        Debug.Log("Used SecurityCard!");
    }
    public override string ToString()
    {
        return itemName + "item";
    }
}
