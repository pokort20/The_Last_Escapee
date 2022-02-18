using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    bool flashlightEnabled;
    public Light flashlight;
    Inventory inventory;
    float batteryLevel;
    float intensityRangeMin;
    float intensityRangeMax;
    float decayThreshold;
    private void Start()
    {
        flashlightEnabled = false;
        flashlight.enabled = flashlightEnabled;
        inventory = GetComponentInParent<Inventory>();
        batteryLevel = 60.0f;
        intensityRangeMin = 0.2f;
        intensityRangeMax = 1.0f;
        decayThreshold = 0.66f*batteryLevel;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && inventory.hasItem(typeof(FlashlightItem)) > 0 && batteryLevel > 0.0f)
        {
            switchFlaslight();
        }
        if (flashlightEnabled)
        {
            if(batteryLevel <= 0.0f)
            {
                batteryLevel = 0.0f;
                switchFlaslight();
                return;
            }
            batteryLevel -= 1.0f * Time.deltaTime;
            updateLightIntensity();
        }
        //Debug.Log("Battery level: " + batteryLevel);
    }
    private void switchFlaslight()
    {
        Debug.Log("Enabled / disabled flashlight");
        flashlightEnabled = !flashlightEnabled;
        flashlight.enabled = flashlightEnabled;
    }
    private void updateLightIntensity()
    {
        if(batteryLevel > decayThreshold)
        {
            flashlight.intensity = 1.0f;
        }
        if(batteryLevel < decayThreshold)
        {

            flashlight.intensity = intensityRangeMin + ((intensityRangeMax-intensityRangeMin)/decayThreshold * batteryLevel);
            if (batteryLevel < 0.5f * decayThreshold)
            {
                float ran = Random.Range(0.0f, 100.0f);
                if (ran > 97.0f)
                {
                    flashlight.intensity *= 0.25f;
                }
            }
        }
    }
}
