using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DisableDirectionalLight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach(Light light in FindObjectsOfType<Light>())
        {
            Debug.Log("SCENE LIGHT: " + light.name);
            if(light.type == LightType.Directional)
            {
                light.enabled = false;
                Debug.Log("Disabled directional light " + light.name);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
