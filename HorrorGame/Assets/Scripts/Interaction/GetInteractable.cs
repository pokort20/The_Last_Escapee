using UnityEngine;

public class GetInteractable : MonoBehaviour
{
    public Camera FirstPersonCamera;
    public float maxInteractionDistance;

    RaycastHit raycastHit;
    GameObject interactingObject;
    void Start()
    {

    }

    void FixedUpdate()
    {
        if (Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out raycastHit, maxInteractionDistance))
        {
            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                interactingObject = raycastHit.collider.gameObject;
                //Debug.Log("Hit interactable object!" + raycastHit.collider.gameObject.ToString());
                if(Input.GetKeyDown(KeyCode.E))
                {
                    interactingObject.GetComponent<Interactable>().Interact();
                    //Debug.Log("Interact!");

                }
            }
        }
    }
}
