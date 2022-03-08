using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class EnemyUtilities : MonoBehaviour
{
    public Transform player;
    public float playerVisibility { get; set; }
    private float bodyArea = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player != null)
        {
            playerVisibility = getPlayerVisibility(player.position);
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
        if (GameManager.instance.flashlightEnabled) playerIlluminationIntensity += 0.5f;
        RaycastHit raycastHit;
        Debug.Log("----------------------------LIGHTS---------------------------");
        foreach (Light light in Resources.FindObjectsOfTypeAll(typeof(Light)))
        {
            Debug.Log("-------------------------------");
            Debug.Log("Light: " + light.name);
            float intensity;
            HDAdditionalLightData lightData = light.GetComponent<HDAdditionalLightData>();
            if (lightData == null)
            {
                //Debug.Log("This light does not have HDAdditionaLightData component!");
                continue;
            }

            if (light.isActiveAndEnabled && Vector3.Distance(light.transform.position, playerPos) < light.range)
            {
                Debug.Log("HD intensity: " + lightData.intensity);

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

        Debug.Log("Player illumination intensity: " + playerIlluminationIntensity);
        return playerIlluminationIntensity;
    }
}
