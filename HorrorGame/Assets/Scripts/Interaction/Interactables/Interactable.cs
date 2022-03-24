using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool canInteract { get; set; }
    public virtual void Interact()
    {
        
    }
    public virtual string InteractText()
    {
        return "Interact";
    }
}
