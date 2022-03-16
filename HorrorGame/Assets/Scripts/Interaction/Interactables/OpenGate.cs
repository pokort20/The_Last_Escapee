using UnityEngine;

public class OpenGate : Interactable
{
    public GameObject Gate;
    public bool isClosed;

    float movementDuration = 3.0f;
    private Vector3 targetPos;
    private Vector3 startPos;
    private float startRot;
    private float targetRot;
    private float elapsedTime;
    private bool isMoving;
    private float percentageMoved;
    void Start()
    {
        setMovementVariables();
    }

    // Update is called once per frame
    public override void Interact()
    {
        base.Interact();
        Debug.Log("Moving gate!");
        isMoving = true;
    }
    public override string InteractText()
    {
        return "Open/close";
    }
    private void setMovementVariables()
    {
        startPos = Gate.transform.position;
        startRot = transform.rotation.z;
        isMoving = false;
        elapsedTime = 0.0f;
        if (isClosed)
        {
            targetPos = new Vector3(startPos.x, startPos.y + 1.9f, startPos.z);
            targetRot = startRot + 360.0f;
        }
        else
        {
            targetPos = new Vector3(startPos.x, startPos.y - 1.9f, startPos.z);
            targetRot = startRot - 360.0f;
        }
    }
    void Update()
    {
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;
            percentageMoved = elapsedTime / movementDuration;
            Gate.transform.position = Vector3.Lerp(startPos, targetPos, Mathf.SmoothStep(0, 1, percentageMoved));
            Debug.Log("Rotation: " + Mathf.SmoothStep(startRot, targetRot, percentageMoved));
            transform.RotateAround(transform.position, transform.forward, targetRot / movementDuration * Time.deltaTime);
            //transform.rotation = Quaternion.AngleAxis(Mathf.Lerp(startRot, targetRot, percentageMoved), Vector3.forward);
            //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Mathf.Lerp(startRot, targetRot, percentageMoved));
            Debug.Log("precentage finished: " + percentageMoved);
            if (percentageMoved >= 1.0f)
            {
                Debug.Log("Movement finished!");
                isClosed = !isClosed;
                setMovementVariables();
            }
        }
    }
}
