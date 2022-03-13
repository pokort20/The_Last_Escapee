using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;
    public GameObject patrolPoints;
    public float enemyFOV;
    public float agentSpeed;
    public float hearingRange;

    private enum enemyStates : int
    {
        PATROL = 0,
        CHASING = 1,
        EXPLORE_AREA = 2
    }

    private enemyStates enemyState;
    // Start is called before the first frame update
    void Start()
    {
        enemyState = enemyStates.PATROL;
        agent.speed = agentSpeed;
    }

    void FixedUpdate()
    {
        switch (enemyState)
        {
            case enemyStates.PATROL:
                //Debug.Log("PATROL");
                patrol();
                break;
            case enemyStates.CHASING:
                //Debug.Log("CHASE");
                chase();
                break;
            case enemyStates.EXPLORE_AREA:
                //Debug.Log("EXPLORE_AREA");
                explore();
                break;
            default:
                //Debug.Log("Enemy state unknown!");
                break;
        }
        //if(isPlayerVisible(agent.transform.position, player.position))
        //{
        //    agent.SetDestination(player.position);
        //}
    }
    private void patrol()
    {
        if (isPlayerVisible(agent.transform.position, player.position) || isPlayerHearable(agent.transform.position, player.position))
        {
            Debug.Log("Player spotted, switching to CHASING!");
            enemyState = enemyStates.CHASING;
            agent.speed = agentSpeed * 2.0f;
            //chase();
            return;
        }
        if(reachedDestination())
        {
            agent.ResetPath();
            Debug.Log("Enemy reached destination!");
        }
        if (!agent.hasPath /*&& !isPlayerVisible(agent.transform.position, player.position)*/)
        {
            //find new patrol point

            Transform selectedPatrolPoint = patrolPoints.transform.GetChild(Random.Range(0, patrolPoints.transform.childCount));
            Debug.Log("Selected patrol point: " + selectedPatrolPoint.name);
            agent.SetDestination(selectedPatrolPoint.position);
            //foreach(Transform point in patrolPoints.transform)
            //{
            //    Debug.Log("Patrol point: " + point.name);
            //}
            //Debug.Log("Enemy is not moving");
        }
    }
    private void chase()
    {
        if (isPlayerVisible(agent.transform.position, player.position) || isPlayerHearable(agent.transform.position, player.position))
        {
            //Still can see or hear player
            agent.SetDestination(player.position);
        }
        else if (agent.hasPath)
        {
            //Can not see, but still going to the last pos where player was seen
            if (reachedDestination())
            {
                agent.ResetPath();
                Debug.Log("Enemy reached chasing destination!");
            }
        }
        else
        {
            //Can't see player nor has destination to go to
            enemyState = enemyStates.EXPLORE_AREA;
            agent.speed = agentSpeed*1.5f;
            Debug.Log("Switching to EXPLORE_AREA!");
        }
    }
    private void explore()
    {
        if (isPlayerVisible(agent.transform.position, player.position) || isPlayerHearable(agent.transform.position, player.position))
        {
            Debug.Log("Player spotted while exploring area, switching to CHASING!");
            enemyState = enemyStates.CHASING;
            agent.speed = agentSpeed * 2.0f;
            chase();
            return;
        }
        if (!agent.hasPath /*&& !isPlayerVisible(agent.transform.position, player.position)*/)
        {
            //find the point in the best direction 
            float bestValue = -1.0f;
            float currentValue;
            Transform toBeExplored = null;
            foreach(Transform patrolPoint in patrolPoints.transform)
            {
                currentValue = Vector3.Angle(agent.transform.forward, patrolPoint.position - agent.transform.position);
                currentValue *= Mathf.Max(1.0f, Vector3.Distance(agent.transform.position, patrolPoint.position));
                if (currentValue < bestValue || bestValue == -1.0f)
                {
                    toBeExplored = patrolPoint;
                    bestValue = currentValue;
                }
            }
            Debug.Log("Selected point to explore: " + toBeExplored.name);
            agent.SetDestination(toBeExplored.position);
        }
        if (reachedDestination())
        {
            Debug.Log("Explored area, returning to PATROL!");
            agent.ResetPath();
            enemyState = enemyStates.PATROL;
            agent.speed = agentSpeed;
            return;
        }
    }
    private bool reachedDestination()
    {
        if(Vector3.Distance(agent.transform.position, agent.destination) < 1.05f * agent.stoppingDistance)
        {
            return true;
        }
        return false;
    }
    private bool isPlayerVisible(Vector3 agentPos, Vector3 playerPos)
    {
        RaycastHit raycastHit;
        float playerVisibility = FindObjectOfType<EnemyUtilities>().playerVisibility;
        float visibilityDistance = remap(0.0f, 70.0f, 2.0f, 15.0f, playerVisibility);
        //Debug.Log("visibility distance: " + visibilityDistance);
        
        Vector3 eyePos = agentPos;
        eyePos.y += 1.2f;
        Vector3 dirToPlayer = playerPos - eyePos;
        float angle = Vector3.Angle(dirToPlayer, agent.transform.forward);
        //Debug.Log("angle: " + angle + ", DirToPlayer: " + dirToPlayer + ", agent fw: " + agent.transform.forward);
        if (Physics.Raycast(eyePos, playerPos - eyePos, out raycastHit, visibilityDistance))
        {
            //Debug.Log("IsPlayerVisible RaycastHit: " + raycastHit.collider.gameObject.name);
            //Debug.Log("AgentPos: " + eyePos + ", PlayerPos: " + playerPos);
            //Debug.Log("Distance: " + Vector3.Distance(eyePos, raycastHit.collider.gameObject.transform.position));
            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Player") && angle < enemyFOV/2)
            {
                //Debug.Log("Enemy sees player!");
                //enemyState = enemyStates.CHASING;
                return true;
            }
        }
        return false;
    }
    private bool isPlayerHearable(Vector3 agentPos, Vector3 playerPos)
    {
        float moveVecMag = player.GetComponent<PlayerMovement>().getPlayerMoveVec().magnitude;
        float noiseRange = moveVecMag * 0.5f;
        noiseRange = Mathf.Pow(noiseRange, 2.0f);
        //Debug.Log("Noise range: " + noiseRange);
        float dist = Vector3.Distance(agentPos, playerPos);
        RaycastHit raycastHit;
        Vector3 earPos = agentPos;
        earPos.y += 1.2f;
        int layerMask = LayerMask.NameToLayer("Walls");

        //Debug.Log("Distance " + (Vector3.Distance(agentPos, playerPos) - hearingRange - noiseRange));
        if (dist < hearingRange + noiseRange)
        {
            if (Physics.Raycast(agentPos, playerPos - agentPos, out raycastHit, hearingRange + noiseRange, layerMask))
            {
                //Debug.Log("Raycast hit: " + raycastHit.collider.gameObject.name);
                //Debug.Log("Wall between enemy and player in hearing range");
                if(3*dist < hearingRange + noiseRange)
                {
                    //Debug.Log("Hears through wall");
                    return true;
                }
                //Debug.Log("Hears, but behind wall");
                return false;
            }
            //Debug.Log("Enemy hears player!");
            return true;
        }
        return false;
    }
    private float remap(float iMin, float iMax, float oMin, float oMax, float value)
    {
        float val;
        val = Mathf.InverseLerp(iMin, iMax, value);
        return Mathf.Lerp(oMin, oMax, val);
    }
}
