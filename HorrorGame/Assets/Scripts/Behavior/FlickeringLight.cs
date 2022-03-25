using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class FlickeringLight : MonoBehaviour
{
    public Light flickeringLight;
    public GameObject emissivePart;
    public bool isFlickeringEnabled;

    private HDAdditionalLightData lightData;
    private float ranNum;
    private float defaultIntensity;
    private Material mat;
    private Color emissiveColor;
    // Start is called before the first frame update
    void Start()
    {
        ranNum = 0.0f;
        lightData = flickeringLight.GetComponent<HDAdditionalLightData>();
        if(emissivePart != null)
        {
            mat = emissivePart.GetComponent<Renderer>().material;
            emissiveColor = mat.GetColor("_EmissiveColor");
        }
        if(lightData == null)
        {
            Debug.LogWarning("FlickeringLight's light has no HDAdditionalLightData");
        }
        else
        {
            defaultIntensity = lightData.intensity;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isFlickeringEnabled)
        {
            if (flickeringLight.enabled)
            {
                ranNum = Random.value;
                //Debug.Log("ranNum: " + ranNum);
                if (ranNum >= 0.985f)
                {
                    lightData.intensity = 0.2f * defaultIntensity;
                    if (emissivePart != null)
                    {
                        mat.SetColor("_EmissiveColor", Color.black);
                    }
                }
                else if(ranNum < 0.45f)
                {
                    lightData.intensity = defaultIntensity;
                    if (emissivePart != null)
                    {
                        mat.SetColor("_EmissiveColor", emissiveColor);
                    }
                }
            }
        }
    }
}
