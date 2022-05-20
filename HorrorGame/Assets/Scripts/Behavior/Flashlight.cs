/// Class that handles flashlight
/**
    This class handles the flashlight mechanic as 
    well as the battery consumption. When the batteries
    are low, the flashlight starts flickering.
*/
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Flashlight : MonoBehaviour
{
    //Public variables defined in Unity inspector.
    public Light flashlight;
    public HDAdditionalLightData lightData;

    //Other variables
    private Inventory inventory;
    private float intensityRangeMin;
    private float intensityRangeMax;
    private float decayThreshold;
    private float flashlightSwitchCooldown;

    //Init
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

    //Update
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
    }

    //Other functions
    private void switchFlaslight()
    {
        if(GameManager.instance.flashlightEnabled)
        {
            AudioManager.instance.playAudio("flashlight_ON");
        }
        else
        {
            if (Tutorial.instance != null)
            {
                Tutorial.instance.hideAllHelps();
            }
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
            if(Tutorial.instance != null)
            {
                Tutorial.instance.showHelp("batteries");
            }
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
