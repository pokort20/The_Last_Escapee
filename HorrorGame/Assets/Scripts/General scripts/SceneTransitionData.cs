using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionData : MonoBehaviour
{
    public float health { get; set; }
    public float stamina { get; set; }
    public float batteryLevel { get; set; }
    public bool flashlightEnabled { get; set; }
    public List<Item> inventoryItems { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        
    }
}
