using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBox : Interactable
{
    public bool isEnabled;
    public GameObject disabledIndicator;
    public GameObject enabledIndicator;
    public Light disabledLight;
    public Light enabledLight;
    public GameObject interactingGameObject;
    // Start is called before the first frame update
    void Start()
    {
        handleIndicator();
        canInteract = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Interact()
    {
        isEnabled = !isEnabled;
        handleIndicator();
        if (interactingGameObject != null)
        {
            OpenSlideDoor openSlideDoor = interactingGameObject.GetComponentInChildren<OpenSlideDoor>();
            if(openSlideDoor != null)
            {
                openSlideDoor.isOpenable = isEnabled;
                return;
            }
            OpenGate openGate = interactingGameObject.GetComponent<OpenGate>();
            if(openGate != null)
            {
                openGate.isOpenable = isEnabled;
                return;
            }

        }
    }
    public override string InteractText()
    {
        if(isEnabled)
        {
            return "Switch OFF";
        }
        else
        {
            return "Switch ON";
        }
       
    }
    private void handleIndicator()
    {
        disabledIndicator.SetActive(!isEnabled);
        disabledLight.enabled = !isEnabled;

        enabledIndicator.SetActive(isEnabled);
        enabledLight.enabled = isEnabled;
    }
}
