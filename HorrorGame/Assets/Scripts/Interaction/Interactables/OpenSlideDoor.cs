using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.HighDefinition;

public class OpenSlideDoor : Interactable
{
    //public variables defined in UNITY
    public Transform leftDoor;
    public Transform rightDoor;
    public GameObject lightObject1;
    public GameObject lightObject2;
    public GameObject emissiveObject1;
    public GameObject emissiveObject2;
    public AudioSource audioSource;
    public bool isClosed;
    public bool isOpenable;
    //public string infoText;

    //other variables
    private float elapsedTime;
    private float movementDuration = 3.0f;
    private float percentageMoved;
    private float percentageDelta;
    private Vector3 leftOpenedPos;
    private Vector3 leftClosedPos;
    private Vector3 rightOpenedPos;
    private Vector3 rightClosedPos;

    private bool lightEnabled;
    private Light light1;
    private Light light2;
    private HDAdditionalLightData lightData1;
    private HDAdditionalLightData lightData2;
    private Material mat1;
    private Material mat2;
    private Color emissiveColor1;
    private Color emissiveColor2;
    private Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        canInteract = true;
        lightEnabled = true;
        light1 = lightObject1.GetComponentInChildren<Light>();
        light2 = lightObject2.GetComponentInChildren<Light>();
        mat1 = emissiveObject1.GetComponent<Renderer>().material;
        mat2 = emissiveObject2.GetComponent<Renderer>().material;
        emissiveColor1 = mat1.GetColor("_EmissiveColor");
        emissiveColor2 = mat2.GetColor("_EmissiveColor");
        if(light1 != null && light2 != null)
        {
            lightData1 = light1.GetComponent<HDAdditionalLightData>();
            lightData2 = light2.GetComponent<HDAdditionalLightData>();
        }
        setMovementVariables();
    }

    // Update is called once per frame
    void Update()
    {
        if(!canInteract)
        {
            //doors are currently moving
            if (isClosed)
            {
                openDoor();
            }
            else
            {
                closeDoor();
            }
        }
    }
    public override void Interact()
    {
        if(isOpenable && inventory.hasItem(typeof(SecurityCardItem)) > 0)
        {
            canInteract = false;
            AudioManager.instance.playAudio("lab_door", audioSource);
            flashingLights();
        }
        else
        {
            if(isOpenable) AudioManager.instance.playAudio("denied");
            if(infoText != null)
            {
                inventory.infoText = infoText;
            }
        }
    }
    private void openDoor()
    {
        elapsedTime += Time.deltaTime;
        percentageMoved = elapsedTime / movementDuration;

        leftDoor.position = Vector3.Lerp(leftClosedPos, leftOpenedPos, Mathf.SmoothStep(0.0f, 1.0f, percentageMoved));
        rightDoor.position = Vector3.Lerp(rightClosedPos, rightOpenedPos, Mathf.SmoothStep(0.0f, 1.0f, percentageMoved));

        if(percentageMoved >= 1.0f)
        {
            lightEnabled = true;
            isClosed = false;
            canInteract = true;
            elapsedTime = 0.0f;
            percentageDelta = 0.0f;
        }
    }
    private void closeDoor()
    {
        elapsedTime += Time.deltaTime;
        percentageMoved = elapsedTime / movementDuration;

        leftDoor.position = Vector3.Lerp(leftOpenedPos, leftClosedPos, Mathf.SmoothStep(0.0f, 1.0f, percentageMoved));
        rightDoor.position = Vector3.Lerp(rightOpenedPos, rightClosedPos, Mathf.SmoothStep(0.0f, 1.0f, percentageMoved));

        if (percentageMoved >= 1.0f)
        {
            lightEnabled = true;
            isClosed = true;
            canInteract = true;
            elapsedTime = 0.0f;
            percentageDelta = 0.0f;
        }
    }
    private void setMovementVariables()
    {
        elapsedTime = 0.0f;
        percentageMoved = 0.0f;
        percentageDelta = 0.0f;
        if(isClosed)
        {
            leftClosedPos = leftDoor.position;
            rightClosedPos = rightDoor.position;
            leftOpenedPos = leftDoor.position + 1.1f * leftDoor.up;
            rightOpenedPos = rightDoor.position - 1.1f * rightDoor.up;
        }
        else
        {
            leftClosedPos = leftDoor.position - 1.1f * leftDoor.up;
            rightClosedPos = rightDoor.position + 1.1f * rightDoor.up;
            leftOpenedPos = leftDoor.position;
            rightOpenedPos = rightDoor.position;
        }
    }
    private void flashingLights()
    {
        for(float i = 0.5f; i < 2.5f; i += 0.5f)
        {
            Invoke("handleLights", i);
        }
    }
    private void handleLights()
    {
        lightEnabled = !lightEnabled;
        light1.enabled = lightEnabled;
        light2.enabled = lightEnabled;

        if (lightEnabled)
        {
            mat1.SetColor("_EmissiveColor", emissiveColor1);
            mat2.SetColor("_EmissiveColor", emissiveColor2);
        }
        else
        {
            mat1.SetColor("_EmissiveColor", Color.black);
            mat2.SetColor("_EmissiveColor", Color.black);
        }

        //percentageDelta += Time.deltaTime;
        //if(percentageDelta / movementDuration >= 0.1 * movementDuration)
        //{
        //    lightEnabled = !lightEnabled;
        //    light1.enabled = lightEnabled;
        //    light2.enabled = lightEnabled;

        //    if(lightEnabled)
        //    {
        //        mat1.SetColor("_EmissiveColor", emissiveColor1);
        //        mat2.SetColor("_EmissiveColor", emissiveColor2);
        //    }
        //    else
        //    {
        //        mat1.SetColor("_EmissiveColor", Color.black);
        //        mat2.SetColor("_EmissiveColor", Color.black);
        //    }

        //    percentageDelta = 0.0f;
        //}
    }
}
