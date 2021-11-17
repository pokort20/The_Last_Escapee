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

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out raycastHit, maxInteractionDistance))
        {
            if (raycastHit.collider.gameObject.layer == 8)
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
