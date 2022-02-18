using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    bool flashlightEnabled;
    public Light flashlight;
    Inventory inventory;
    private void Start()
    {
        flashlightEnabled = false;
        flashlight.enabled = flashlightEnabled;
        inventory = GetComponentInParent<Inventory>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && inventory.hasItem(typeof(FlashlightItem)) > 0)
        {
            Debug.Log("Enabled / disabled flashlight");
            flashlightEnabled = !flashlightEnabled;
            flashlight.enabled = flashlightEnabled;
        }
    }
}
