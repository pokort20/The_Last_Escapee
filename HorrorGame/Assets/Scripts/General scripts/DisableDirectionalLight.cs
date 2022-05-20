/// Disable directional light class
/**
    This class is used for development only. It disables
    directional lights used in scene during the game's
    development.
*/
using UnityEngine;

public class DisableDirectionalLight : MonoBehaviour
{
    //Init
    void Start()
    {
        foreach(Light light in FindObjectsOfType<Light>())
        {
            if(light.type == LightType.Directional)
            {
                light.enabled = false;
                Debug.Log("Disabled directional light " + light.name);
            }
        }
    }
}
