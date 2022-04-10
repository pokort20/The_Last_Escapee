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

    // Update is called once per frame
    void Update()
    {
        //if (GameManager.instance.stamina < GameManager.instance._maxStamina / 2)
        //{
        //    isUseable = true;
        //}
        //else
        //{
        //    isUseable = false;
        //}
    }
    public override void UseItem()
    {
        GameManager.instance.stamina = GameManager.instance._maxStamina;
        Debug.Log("Used Stamina shot!");
    }
    public override string ToString()
    {
        return itemName + "item";
    }
}
