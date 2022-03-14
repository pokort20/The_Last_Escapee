using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : Interactable
{
    public Transform door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Interact()
    {
        //base.Interact();
        door.RotateAround(door.position, new Vector3(0.0f, 1.0f, 0.0f), 45.0f);
    }

}
