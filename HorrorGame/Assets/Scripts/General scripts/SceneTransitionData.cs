/// Scene transition data class
/**
    This class serves as a struct, encapsuling
    variables required to transition between scenes.
*/
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionData : MonoBehaviour
{
    //Properties
    public float health { get; set; }
    public float stamina { get; set; }
    public float batteryLevel { get; set; }
    public bool flashlightEnabled { get; set; }
    public List<Item> inventoryItems { get; set; }
}
