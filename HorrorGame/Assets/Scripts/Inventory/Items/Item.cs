using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite icon;
    public bool oneTimeUse = true;

    public virtual void UseItem()
    {

    }
}
