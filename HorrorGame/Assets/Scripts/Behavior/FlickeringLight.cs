/// Flickering light class
/**
    This class handles the flickering light. It is
    applied as a component to flickering lights
    in the scene.
*/
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class FlickeringLight : MonoBehaviour
{
    //Public variables defined in Unity inspector
    public Light flickeringLight;
    public GameObject emissivePart;
    public bool isFlickeringEnabled;
    public AudioSource audioSource;

    //Other variables
    private HDAdditionalLightData lightData;
    private float ranNum;
    private float defaultIntensity;
    private Material mat;
    private Color emissiveColor;

    //Init
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
        if(audioSource == null)
        {
            Debug.LogWarning("Flickering light script has no audio source attached!");
        }
    }

    //Update
    void FixedUpdate()
    {
        if(isFlickeringEnabled)
        {
            if (flickeringLight.enabled)
            {
                ranNum = Random.value;
                if (ranNum >= 0.997f)
                {
                    AudioManager.instance.playAudio("flicker", audioSource);
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
