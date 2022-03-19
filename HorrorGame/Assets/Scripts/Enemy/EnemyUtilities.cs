using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class EnemyUtilities : MonoBehaviour
{
    public Transform player;
    public float playerVisibility { get; set; }
    public List<Vector3> flashlightHitPoints { get; set; }

    private float bodyArea = 2.0f;
    private HDAdditionalLightData flashlight;

    // Start is called before the first frame update
    void Start()
    {
        flashlight = player.GetComponentInChildren<HDAdditionalLightData>();
        if (flashlight == null)
        {
            Debug.LogError("Enemy can't access flashlight!");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player != null)
        {
            playerVisibility = getPlayerVisibility(player.position);
            if(GameManager.instance.flashlightEnabled)
            {
                flashlightHitPoints = getFlashlightHitPoints(flashlight);
            }
            else
            {
                flashlightHitPoints = null;
            }
        }
        else
        {
            Debug.Log("Unknown referrence to player, attach player to EnemyUtilities script in inspector!");
        }

    }
    private float getPlayerVisibility(Vector3 playerPos)
    {
        bool debugRays = true;
        float playerIlluminationIntensity = 0.0f;
        if (GameManager.instance.flashlightEnabled) playerIlluminationIntensity += 40.0f;
        RaycastHit raycastHit;
        //Debug.Log("----------------------------LIGHTS---------------------------");
        foreach (Light light in Resources.FindObjectsOfTypeAll(typeof(Light)))
        {
            //Debug.Log("-------------------------------");
            //Debug.Log("Light: " + light.name);
            float intensity;
            HDAdditionalLightData lightData = light.GetComponent<HDAdditionalLightData>();
            if (lightData == null)
            {
                //Debug.Log("This light does not have HDAdditionaLightData component!");
                continue;
            }

            if (light.isActiveAndEnabled && Vector3.Distance(light.transform.position, playerPos) < light.range)
            {
                //Debug.Log("HD intensity: " + lightData.intensity);

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
                    if (Vector3.Angle(lightData.transform.forward, playerPos - lightData.transform.position) < lightData.lightAngle * 0.5f)
                    {
                        if (Physics.Raycast(lightData.transform.position, playerPos - lightData.transform.position, out raycastHit, lightData.range))
                        {
                            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                            {
                                //Spot light shines on player
                                if (debugRays) Debug.DrawRay(light.transform.position, Vector3.Normalize(playerPos - lightData.transform.position) * lightData.range, Color.green);
                                
                                intensity = Mathf.Clamp(bodyArea * lightData.intensity / Mathf.Pow(Vector3.Distance(lightData.transform.position, playerPos), 2.0f), 0.0f, lightData.intensity);
                                playerIlluminationIntensity += intensity;
                                continue;
                            }
                        }
                    }
                }
                if (debugRays) Debug.DrawRay(lightData.transform.position, Vector3.Normalize(playerPos - lightData.transform.position) * lightData.range, Color.red);
            }
        }

        //Debug.Log("Player illumination intensity: " + playerIlluminationIntensity);
        return playerIlluminationIntensity;
    }
    private List<Vector3> getFlashlightHitPoints(HDAdditionalLightData flashlight)
    {
        //general variables
        List<Vector3> flashlightHitPoints = new List<Vector3>();
        bool debugFlashlightRays = true;
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
            if(Physics.Raycast(origin: flashlight.transform.position, direction: pointsOnCircle[i] - flashlight.transform.position, hitInfo: out raycastHit, maxDistance: flashlight.range))
            {
                flashlightHitPoints.Add(raycastHit.point);
            }
        }

        //Drawing rays for debug purposes
        if (debugFlashlightRays && GameManager.instance.flashlightEnabled)
        {
            foreach(Vector3 hitPoint in flashlightHitPoints)
            {
                Debug.DrawRay(flashlight.transform.position, hitPoint - flashlight.transform.position, Color.white);
            }
            //for (int i = 0; i < 7; ++i)
            //{
            //    Vector3 dir = Vector3.Normalize(pointsOnCircle[i] - flashlight.transform.position);
            //    Debug.DrawRay(flashlight.transform.position, dir * flashlight.range, Color.white);
            //}
        }


        return flashlightHitPoints;
    }
}
