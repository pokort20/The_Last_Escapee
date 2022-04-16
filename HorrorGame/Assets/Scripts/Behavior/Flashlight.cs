using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Flashlight : MonoBehaviour
{
    public Light flashlight;
    public HDAdditionalLightData lightData;
    Inventory inventory;
    float intensityRangeMin;
    float intensityRangeMax;
    float decayThreshold;
    private void Start()
    {
        GameManager.instance.flashlightEnabled = false;
        flashlight.enabled = GameManager.instance.flashlightEnabled;
        inventory = Inventory.instance;
        intensityRangeMin = 12.0f;
        intensityRangeMax = 60.0f;
        decayThreshold = 0.5f * GameManager.instance._maxBatteryLevel;

        lightData = flashlight.GetComponent<HDAdditionalLightData>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && inventory.hasItem(typeof(FlashlightItem)) > 0 && GameManager.instance.batteryLevel > 0.0f)
        {
            switchFlaslight();
        }
        if (GameManager.instance.flashlightEnabled)
        {
            if(GameManager.instance.batteryLevel <= 0.0f)
            {
                GameManager.instance.batteryLevel = 0.0f;
                switchFlaslight();
                return;
            }
            GameManager.instance.batteryLevel -= 1.0f * Time.deltaTime;
            updateLightIntensity();
        }
        //Debug.Log("Battery level: " + GameManager.instance.batteryLevel);
    }
    private void switchFlaslight()
    {
        Debug.Log("Enabled / disabled flashlight");
        GameManager.instance.flashlightEnabled = !GameManager.instance.flashlightEnabled;
        flashlight.enabled = GameManager.instance.flashlightEnabled;
    }
    private void updateLightIntensity()
    {
        if(GameManager.instance.batteryLevel > decayThreshold)
        {
            lightData.intensity = intensityRangeMax;
        }
        if(GameManager.instance.batteryLevel < decayThreshold)
        {

            lightData.intensity = intensityRangeMin + ((intensityRangeMax-intensityRangeMin)/decayThreshold * GameManager.instance.batteryLevel);
            if (GameManager.instance.batteryLevel < 0.5f * decayThreshold)
            {
                float ran = Random.Range(0.0f, 100.0f);
                if (ran > 97.0f)
                {
                    lightData.intensity *= 0.25f;
                }
            }
        }
    }
}
