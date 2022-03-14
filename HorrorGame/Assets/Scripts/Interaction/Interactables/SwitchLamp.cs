using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLamp : Interactable
{
    public GameObject lampParent;

    // Start is called before the first frame update
    bool isEnabled;
    void Update()
    {
        isEnabled = lampParent.transform.GetChild(0).transform.GetChild(0).gameObject.activeInHierarchy;
    }
    public override void Interact()
    {
        Debug.Log("Enabled / disabled " + lampParent.name + " lights!");
        isEnabled = !isEnabled;
        foreach (Transform FluorescentLamp in lampParent.transform)
        {
            for(int i = 0; i < FluorescentLamp.childCount; ++i)
            {
                FluorescentLamp.GetChild(i).gameObject.SetActive(isEnabled);
            }
        }
    }
}
