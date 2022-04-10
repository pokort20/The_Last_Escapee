using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public string itemInfo;
    public Sprite icon;
    public bool isUseable = true;

    public virtual void UseItem()
    {

    }
}
