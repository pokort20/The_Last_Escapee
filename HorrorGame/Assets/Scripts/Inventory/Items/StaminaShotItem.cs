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

    }
    public override void UseItem()
    {
        Debug.Log("Used Stamina shot!");
    }
    public override string ToString()
    {
        return itemName + "item";
    }
}
