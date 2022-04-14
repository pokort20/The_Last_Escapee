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
        canInteract = false;
        if(interactingGameObject != null)
        {
            OpenSlideDoor openSlideDoor = interactingGameObject.GetComponentInChildren<OpenSlideDoor>();
            if(openSlideDoor != null)
            {
                openSlideDoor.isOpenable = true;
            }
            else
            {
                Debug.LogWarning("Can not find OpenSlideDoor script on the interacting game object");
            }
        }
        //base.Interact();
    }
    public override string InteractText()
    {
        return "Switch ON";
    }
    private void handleIndicator()
    {
        disabledIndicator.SetActive(!isEnabled);
        disabledLight.enabled = !isEnabled;

        enabledIndicator.SetActive(isEnabled);
        enabledLight.enabled = isEnabled;
    }
}
