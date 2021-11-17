using UnityEngine;

public class OpenGate : Interactable
{
    public GameObject Gate;
    public bool isClosed;

    float movementDuration = 2.0f;
    private Vector3 targetPos;
    private Vector3 startPos;
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
    private void setMovementVariables()
    {
        startPos = Gate.transform.position;
        isMoving = false;
        elapsedTime = 0.0f;
        if (isClosed)
        {
            targetPos = new Vector3(startPos.x, startPos.y + 1.9f, startPos.z);
        }
        else
        {
            targetPos = new Vector3(startPos.x, startPos.y - 1.9f, startPos.z);
        }
    }
    void Update()
    {
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;
            percentageMoved = elapsedTime / movementDuration;
            Gate.transform.position = Vector3.Lerp(startPos, targetPos, Mathf.SmoothStep(0, 1, percentageMoved));
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
