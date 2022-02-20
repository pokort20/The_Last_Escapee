using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryItem : Item
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void UseItem()
    {
        Debug.Log("Used battery!");
    }
    public override string ToString()
    {
        return itemName + "item";
    }
}
