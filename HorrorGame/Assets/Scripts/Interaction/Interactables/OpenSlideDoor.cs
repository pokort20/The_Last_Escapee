using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class OpenSlideDoor : Interactable
{
    public Transform leftDoor;
    public Transform rightDoor;
    public GameObject lightObject1;
    public GameObject lightObject2;
    public bool isClosed;

    private float elapsedTime;
    private float movementDuration = 2.0f;
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
    // Start is called before the first frame update
    void Start()
    {
        canInteract = true;
        lightEnabled = true;
        light1 = lightObject1.GetComponentInChildren<Light>();
        light2 = lightObject2.GetComponentInChildren<Light>();
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
        canInteract = false;
    }
    private void openDoor()
    {
        elapsedTime += Time.deltaTime;
        percentageMoved = elapsedTime / movementDuration;

        leftDoor.position = Vector3.Lerp(leftClosedPos, leftOpenedPos, Mathf.SmoothStep(0.0f, 1.0f, percentageMoved));
        rightDoor.position = Vector3.Lerp(rightClosedPos, rightOpenedPos, Mathf.SmoothStep(0.0f, 1.0f, percentageMoved));
        handleLights();

        if(percentageMoved >= 1.0f)
        {
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
        handleLights();

        if (percentageMoved >= 1.0f)
        {
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
    private void handleLights()
    {
        percentageDelta += Time.deltaTime;
        if(percentageDelta / movementDuration >= 0.1 * movementDuration)
        {
            lightEnabled = !lightEnabled;
            light1.enabled = lightEnabled;
            light2.enabled = lightEnabled;

            
            percentageDelta = 0.0f;
        }
    }
}
