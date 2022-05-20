/// Get interactable class - not used in the final project, merged with FindInteraction
using UnityEngine;

public class GetInteractable : MonoBehaviour
{
    public Camera FirstPersonCamera;
    public float maxInteractionDistance;

    RaycastHit raycastHit;
    GameObject interactingObject;
    private bool keyPressed;
    void Start()
    {
        keyPressed = false;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            keyPressed = true;
        }
        if(Input.GetKeyUp(KeyCode.E))
        {
            keyPressed = false;
        }
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out raycastHit, maxInteractionDistance))
        {
            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                interactingObject = raycastHit.collider.gameObject;
                //Debug.Log("Hit interactable object!" + raycastHit.collider.gameObject.ToString());
                if(keyPressed)
                {
                    interactingObject.GetComponent<Interactable>().Interact();
                    //Debug.Log("Interact!");
                    keyPressed = false;
                }
            }
        }
    }
}
