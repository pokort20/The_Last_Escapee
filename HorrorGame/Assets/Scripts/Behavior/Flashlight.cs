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
    float flashlightSwitchCooldown;
    private void Start()
    {
        flashlight.enabled = GameManager.instance.flashlightEnabled;
        inventory = Inventory.instance;
        intensityRangeMin = 12.0f;
        intensityRangeMax = 60.0f;
        decayThreshold = 0.5f * GameManager.instance._maxBatteryLevel;
        flashlightSwitchCooldown = 0.0f;
        lightData = flashlight.GetComponent<HDAdditionalLightData>();
    }
    // Update is called once per frame
    void Update()
    {
        flashlightSwitchCooldown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.F) && inventory.hasItem(typeof(FlashlightItem)) > 0 && GameManager.instance.batteryLevel > 0.0f)
        {
            if(flashlightSwitchCooldown <= 0.0)
            {
                switchFlaslight();
                flashlightSwitchCooldown = 0.5f;
            }
        }
        if (GameManager.instance.flashlightEnabled)
        {
            if(GameManager.instance.batteryLevel <= 0.0f)
            {
                GameManager.instance.batteryLevel = 0.0f;
                switchFlaslight();
                return;
            }
            GameManager.instance.batteryLevel -= 0.5f * Time.deltaTime;
            updateLightIntensity();
        }
        //Debug.Log("Battery level: " + GameManager.instance.batteryLevel);
    }
    private void switchFlaslight()
    {
        if(GameManager.instance.flashlightEnabled)
        {
            Debug.Log("Disabled flashlight");
            AudioManager.instance.playAudio("flashlight_ON");
        }
        else
        {
            Debug.Log("Eisabled flashlight");
            AudioManager.instance.playAudio("flashlight_OFF");
        }
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
