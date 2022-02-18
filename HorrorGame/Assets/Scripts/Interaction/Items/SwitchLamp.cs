using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLamp : Interactable
{
    public GameObject lampParent;

    // Start is called before the first frame update
    bool isEnabled;
    void Start()
    {
        isEnabled = lampParent.transform.GetChild(0).transform.GetChild(0).gameObject.activeInHierarchy;
    }
    public override void Interact()
    {
        Debug.Log("Enabled / disabled " + lampParent.name + " lights!");
        isEnabled = !isEnabled;
        foreach (Transform FluorescentLamp in lampParent.transform)
        {
            FluorescentLamp.GetChild(0).gameObject.SetActive(isEnabled);
            FluorescentLamp.GetChild(1).gameObject.SetActive(isEnabled);
            FluorescentLamp.GetChild(2).gameObject.SetActive(isEnabled);
            FluorescentLamp.GetChild(3).gameObject.SetActive(isEnabled);
        }
    }
}
