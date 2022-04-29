using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaShotItem : Item
{
    // Start is called before the first frame update
    void Awake()
    {
        itemName = "Stamina shot";
    }
    public override void UseItem()
    {
        base.UseItem();
        GameManager.instance.stamina = GameManager.instance._maxStamina;
        Debug.Log("Used Stamina shot!");
    }
    public override string ToString()
    {
        return itemName + "item";
    }
}
