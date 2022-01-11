using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if(isPlayerVisible(agent.transform.position, player.position))
        {
            agent.SetDestination(player.position);
        }

    }
    private bool isPlayerVisible(Vector3 agentPos, Vector3 playerPos)
    {
        RaycastHit raycastHit;
        float visibilityDistance = 10.0f;
        Vector3 eyePos = agentPos;
        eyePos.y += 1.2f;
        if (Physics.Raycast(eyePos, playerPos - eyePos, out raycastHit, visibilityDistance))
        {
            //Debug.Log("AgentPos: " + eyePos + ", PlayerPos: " + playerPos);
            //Debug.Log("Distance: " + Vector3.Distance(eyePos, raycastHit.collider.gameObject.transform.position));
            if (raycastHit.collider.gameObject.layer == 10)
            {
                Debug.Log("Enemy sees player!");
                return true;
                /*
                interactingObject = raycastHit.collider.gameObject;
                //Debug.Log("Hit interactable object!" + raycastHit.collider.gameObject.ToString());
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactingObject.GetComponent<Interactable>().Interact();
                    //Debug.Log("Interact!");

                }
                */
            }
        }
        return false;
    }
}
