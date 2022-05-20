/// Post processing class
/**
    This class handles the game's post processing.
    Other scripts access this class' functions in 
    order to apply the desired post porcessing
    effect.
*/
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PostProcessing : MonoBehaviour
{
    //Singleton
    public static PostProcessing instance;

    //public variables defined in Unity inspector
    public bool mainMenu;
    public Volume globalVolume;
    public float maxLensDistortion;
    public float maxMotionBlur;

    //Post processing overrides
    private GameManager gameManager;
    private LensDistortion lensDistortion;
    private Bloom bloom;
    private DepthOfField depthOfField;
    private MotionBlur motionBlur;
    private GradientSky gradientSky;
    private float _brightness;

    //Properties
    public bool lensDistortionEnabled { get; set; }
    public bool depthOfFieldEnabled { get; set; }
    public bool motionBlurEnabled { get; set; }
    public float previousGradientSkyValue { get; set; }
    public float brightness
    {
        get
        {
            return _brightness;
        }
        set
        {
            _brightness = value;
            handleGradientSky();
        }
    }
    void Awake()
    {
        Debug.Log("Started PostProcessing");
        if (instance != null)
        {
            Debug.LogError("Multiple instances of PostProcessing!!!");
            return;
        }
        instance = this;

        previousGradientSkyValue = 0.5f;
    }
    void Start()
    {
        gameManager = GameManager.instance;
        globalVolume.profile.TryGet(out lensDistortion);
        globalVolume.profile.TryGet(out motionBlur);
        globalVolume.profile.TryGet(out gradientSky);
        lensDistortionEnabled = false;
        depthOfFieldEnabled = false;
        motionBlurEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!mainMenu)
        {
            handleLensDistortion();
            handleMotionBlur();
        }
    }
    private void handleLensDistortion()
    {
        if (lensDistortion == null)
        {
            Debug.LogError("PostProcessing volume has no lensDistortion override!!!");
            return;
        }
        if (lensDistortionEnabled)
        {
            lensDistortion.intensity.value -= 2.0f * Time.deltaTime;
            if (lensDistortion.intensity.value < maxLensDistortion)
            {
                lensDistortion.intensity.value = maxLensDistortion;
            }
        }
        else
        {
            lensDistortion.intensity.value += 2.0f * Time.deltaTime;
            if (lensDistortion.intensity.value > 0.0f)
            {
                lensDistortion.intensity.value = 0.0f;
            }

        }
    }
    //private void handleDepthOfField()
    //{
    //    if (depthOfField == null)
    //    {
    //        Debug.LogError("PostProcessing volume has no depthOfField override!!!");
    //        return;
    //    }

    //    if(depthOfFieldEnabled)
    //    {
    //        depthOfFieldEnabled = false;
    //        depthOfField.active = true;
    //        depthOfField.focusDistance.value = 1.0f;
    //        //depthOfField.focusMode.value = DepthOfFieldMode.UsePhysicalCamera;
    //    }
    //    if(depthOfField.active)
    //    {
    //        depthOfField.focusDistance.value -= 0.1f * Time.deltaTime;
    //        if (depthOfField.focusDistance.value <= 0.1f)
    //        {
    //            depthOfField.focusDistance.value = 0.1f;
    //            depthOfField.active = false;
    //        }
    //    }
    //}
    private void handleMotionBlur()
    {
        if (motionBlur == null)
        {
            Debug.LogError("PostProcessing volume has no motionBlur override!!!");
            return;
        }
        if (motionBlurEnabled)
        {
            motionBlur.intensity.value += 16.0f * Time.deltaTime;
            if (motionBlur.intensity.value > maxMotionBlur)
            {
                motionBlur.intensity.value = maxMotionBlur;
            }
        }
        else
        {
            motionBlur.intensity.value -= 16.0f * Time.deltaTime;
            if (motionBlur.intensity.value < 0.0f)
            {
                motionBlur.intensity.value = 0.0f;
            }
        }
    }
    //public void handleBloom()
    //{
    //    if(gameManager.health < gameManager._maxHealth * 0.5f)
    //    {
    //        bloom.active = true;
    //        bloom.tint.value = Color.HSVToRGB(0.0f, (gameManager._maxHealth - 10.0f - gameManager.health) * 0.01f, 0.4f);
    //        bloom.intensity.value = 0.6f - remap(0.0f, 50.0f, 0.0f, 0.2f, gameManager.health);
    //    }
    //    else
    //    {
    //        bloom.active = false;
    //    }
    //}
    public void handleGradientSky()
    {
        if(gradientSky == null)
        {
            if(!globalVolume.profile.TryGet(out gradientSky))
            {
                Debug.LogError("Gradient sky is null");
                return;
            }
        }
        
        float delta = brightness - previousGradientSkyValue;
        delta *= 0.025f;
        Debug.Log("changing gradient sky, delta: " + delta +  ", brightness: " + brightness + ", previousGradientSkyValue: " + previousGradientSkyValue);

        Color topColor = gradientSky.top.value;
        Color midColor = gradientSky.middle.value;
        Color botColor = gradientSky.bottom.value;

        float h;
        float s;
        float v;
        Color.RGBToHSV(topColor, out h, out s, out v);
        topColor = Color.HSVToRGB(h, s, v + delta);

        Color.RGBToHSV(midColor, out h, out s, out v);
        midColor = Color.HSVToRGB(h, s, v + delta);

        Color.RGBToHSV(botColor, out h, out s, out v);
        botColor = Color.HSVToRGB(h, s, v + delta);

        gradientSky.top.value = topColor;
        gradientSky.middle.value = midColor;
        gradientSky.bottom.value = botColor;

        previousGradientSkyValue = brightness;
    }
    //private float remap(float iMin, float iMax, float oMin, float oMax, float value)
    //{
    //    float val;
    //    val = Mathf.InverseLerp(iMin, iMax, value);
    //    return Mathf.Lerp(oMin, oMax, val);
    //}
}
