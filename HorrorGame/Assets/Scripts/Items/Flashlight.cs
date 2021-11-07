using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    bool flashlightEnabled = true;
    public Light flashlight;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.enabled = flashlightEnabled;
            flashlightEnabled = !flashlightEnabled;

        }
    }
}
