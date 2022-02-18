using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    bool flashlightEnabled;
    public Light flashlight;
    private void Start()
    {
        flashlightEnabled = true;
        flashlight.enabled = flashlightEnabled;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Enabled / disabled flashlight");
            flashlightEnabled = !flashlightEnabled;
            flashlight.enabled = flashlightEnabled;
        }
    }
}
