/// Enemy utilities class
/**
    This class serves as a helper class for enemies. It computes
    the flashlight hitpoints as well as player's visibility. Every
    enemy then accesses this class to retrieve these values.
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class EnemyUtilities : MonoBehaviour
{
    //Public variables defined in Unity inspector
    public Transform player;

    //Properties
    public float playerVisibility { get; set; }
    public List<Vector3> flashlightHitPoints { get; set; }
    public List<int> enemiesStruckByFlashlight { get; set; }

    //Other variables
    private float bodyArea = 2.0f;
    private HDAdditionalLightData flashlight;

    //Init
    void Start()
    {
        flashlight = player.GetComponentInChildren<HDAdditionalLightData>();
        if (flashlight == null)
        {
            Debug.LogError("Enemy can't access flashlight!");
        }
    }

    //Physics update
    void FixedUpdate()
    {
        if(player != null)
        {
            playerVisibility = getPlayerVisibility(player.position);
            if(GameManager.instance.flashlightEnabled)
            {
                enemiesStruckByFlashlight = new List<int>();
                flashlightHitPoints = getFlashlightHitPoints(flashlight);
            }
            else
            {
                enemiesStruckByFlashlight = null;
                flashlightHitPoints = null;
            }
        }
        else
        {
            Debug.Log("Unknown referrence to player, attach player to EnemyUtilities script in inspector!");
        }

    }

    //Other functions
    private float getPlayerVisibility(Vector3 playerPos)
    {
        bool debugRays = true;
        float playerIlluminationIntensity = 0.0f;
        if (GameManager.instance.flashlightEnabled) playerIlluminationIntensity += 40.0f;
        RaycastHit raycastHit;
        foreach (Light light in Resources.FindObjectsOfTypeAll(typeof(Light)))
        {
            float intensity;
            HDAdditionalLightData lightData = light.GetComponent<HDAdditionalLightData>();
            if (lightData == null)
            {
                //Not a HDRP light, ignored
                continue;
            }

            if (light.isActiveAndEnabled && Vector3.Distance(light.transform.position, playerPos) < light.range)
            {
                if (lightData.type == HDLightType.Point)
                {
                    if (Physics.Raycast(lightData.transform.position, playerPos - lightData.transform.position, out raycastHit, lightData.range))
                    {
                        if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                        {
                            //Point Light shines on player
                            if (debugRays) Debug.DrawRay(lightData.transform.position, Vector3.Normalize(playerPos - lightData.transform.position) * lightData.range, Color.green);

                            intensity = Mathf.Clamp(bodyArea * lightData.intensity / Mathf.Pow(Vector3.Distance(lightData.transform.position, playerPos), 2.0f), 0.0f, lightData.intensity);
                            playerIlluminationIntensity += intensity;
                            continue;
                        }
                    }
                }
                else if (lightData.type == HDLightType.Spot)
                {
                    if (lightData.gameObject.name == "FlashLight") continue;
                    Debug.DrawRay(light.transform.position, light.transform.forward, Color.cyan);
                    if (Vector3.Angle(light.transform.forward, playerPos - light.transform.position) < light.spotAngle * 0.5f)
                    {
                        if (Physics.Raycast(light.transform.position, playerPos - light.transform.position, out raycastHit, lightData.range))
                        {
                            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                            {
                                //Spot light shines on player
                                if (debugRays) Debug.DrawRay(light.transform.position, Vector3.Normalize(playerPos - light.transform.position) * lightData.range, Color.green);
                                
                                intensity = Mathf.Clamp(bodyArea * lightData.intensity / Mathf.Pow(Vector3.Distance(light.transform.position, playerPos), 2.0f), 0.0f, lightData.intensity);
                                playerIlluminationIntensity += intensity;
                                continue;
                            }
                        }
                    }
                }
                if (debugRays) Debug.DrawRay(lightData.transform.position, Vector3.Normalize(playerPos - lightData.transform.position) * lightData.range, Color.red);
            }
        }
        if(Tutorial.instance != null && playerIlluminationIntensity > 60.0f)
        {
            Tutorial.instance.showHelp("light");
        }
        return playerIlluminationIntensity;
    }
    private List<Vector3> getFlashlightHitPoints(HDAdditionalLightData flashlight)
    {
        //general variables
        List<Vector3> flashlightHitPoints = new List<Vector3>();
        //bool debugFlashlightRays = false;
        RaycastHit raycastHit;
        //helper variables for defining the points on the circle at the end of the spot light's cone
        float angleCenterToSide = 40.0f * 0.5f;
        float radius = Mathf.Tan(angleCenterToSide * Mathf.Deg2Rad) * flashlight.range;
        float sqrt3 = Mathf.Sqrt(3.0f);

        //defining all the points on the circle of the spot light's cone
        Vector3 circleCenter = flashlight.transform.position + flashlight.transform.forward * flashlight.range;
        Vector3[] pointsOnCircle = new Vector3[7];
        for(int i = 0; i < 7; ++i)
        {
            pointsOnCircle[i] = circleCenter;
        }
        pointsOnCircle[1] += flashlight.transform.right * radius;
        pointsOnCircle[2] += Vector3.Normalize(sqrt3 * flashlight.transform.up + flashlight.transform.right) * radius;
        pointsOnCircle[3] += Vector3.Normalize(sqrt3 * flashlight.transform.up - flashlight.transform.right) * radius;
        pointsOnCircle[4] += - flashlight.transform.right * radius;
        pointsOnCircle[5] += Vector3.Normalize(sqrt3 * -flashlight.transform.up - flashlight.transform.right) * radius;
        pointsOnCircle[6] += Vector3.Normalize(sqrt3 * -flashlight.transform.up + flashlight.transform.right) * radius;

        //approximating the area where the light from the flashlight shines using the few points above,
        //getting the collision points with the environment
        for(int i = 0; i < 7; ++i)
        {
            if(Physics.Raycast(origin: flashlight.transform.position, direction: pointsOnCircle[i] - flashlight.transform.position, hitInfo: out raycastHit, maxDistance: flashlight.range*0.5f))
            {
                flashlightHitPoints.Add(raycastHit.point);

                if(raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    enemiesStruckByFlashlight.Add(raycastHit.collider.gameObject.transform.parent.gameObject.GetHashCode());
                }
            }
        }

        //Drawing rays for debug purposes
        //if (debugFlashlightRays && GameManager.instance.flashlightEnabled)
        //{
        //    foreach(Vector3 hitPoint in flashlightHitPoints)
        //    {
        //        Debug.DrawRay(flashlight.transform.position, hitPoint - flashlight.transform.position, Color.white);
        //    }
        //}
        return flashlightHitPoints;
    }
}
