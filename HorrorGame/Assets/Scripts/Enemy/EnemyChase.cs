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
    public AudioSource loopAudioSource;
    public AudioSource effectAudioSource;

    private Vector3 flashlightVisiblePoint;
    private bool isPrevDestFromFlashlight;
    private float attackCooldown;
    private PostProcessing postProcessing;
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
        postProcessing = PostProcessing.instance;
        flashlightVisiblePoint = Vector3.zero;
        isPrevDestFromFlashlight = false;
        enemyState = enemyStates.PATROL;
        agent.speed = agentSpeed;
        attackCooldown = 0.0f;

        EnemyAudio ea = new EnemyAudio(GetHashCode(), agent.velocity, agent.transform.position, loopAudioSource, effectAudioSource);
        AudioManager.instance.addEnemy(ea);
        Invoke("playRandomSound", Random.Range(5, 15));
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
        AudioManager.instance.updateEnemy(GetHashCode(), agent.transform.position, agent.velocity);
        isPlayerAttacked();
    }
    private void patrol()
    {
        if (isPlayerVisible(agent.transform.position, player.position) || isPlayerHearable(agent.transform.position, player.position)
            || isFlashlightHitPointVisible(agent.transform.position, player.position))
        {
            Debug.Log("Player spotted, switching to CHASING!");
            enemyState = enemyStates.CHASING;
            agent.speed = agentSpeed * 2.0f;
            playAudioOnChaseBegin();
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
            isPrevDestFromFlashlight = false;
            agent.SetDestination(player.position);
        }
        else if(isFlashlightHitPointVisible(agent.transform.position, player.position) && isPrevDestFromFlashlight)
        {
            isPrevDestFromFlashlight = true;
            agent.SetDestination(flashlightVisiblePoint);
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
        if (isPlayerVisible(agent.transform.position, player.position) || isPlayerHearable(agent.transform.position, player.position)
            || isFlashlightHitPointVisible(agent.transform.position, player.position))
        {
            Debug.Log("Player spotted while exploring area, switching to CHASING!");
            enemyState = enemyStates.CHASING;
            agent.speed = agentSpeed * 2.0f;
            playAudioOnChaseBegin();
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
            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Player") && angle < enemyFOV * 0.5f)
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
    private bool isFlashlightHitPointVisible(Vector3 agentPos, Vector3 playerPos)
    {
        bool isFlashlightHPVisible = false;
        float angle;
        Vector3 eyePos = agentPos;
        eyePos.y += 1.2f;
        RaycastHit raycastHit;
        List<Vector3> flashlightHitPoints = FindObjectOfType<EnemyUtilities>().flashlightHitPoints;
        if(flashlightHitPoints == null)
        {
            //Debug.Log("There are no flashlight hit points!");
            return false;
        }

        foreach(Vector3 flashlightHitPoint in flashlightHitPoints)
        {
            angle = Vector3.Angle(flashlightHitPoint - eyePos, agent.transform.forward);
            if(angle > enemyFOV * 0.5f)
            {
                continue;
            }
            if(!Physics.Raycast(origin: eyePos, direction: flashlightHitPoint - eyePos, hitInfo: out raycastHit, maxDistance: Vector3.Distance(eyePos, flashlightHitPoint) - 0.05f))
            {
                //Debug.Log("Enemy sees flashlight hit point " + flashlightHitPoint.ToString());
                Debug.DrawRay(eyePos, flashlightHitPoint - eyePos, Color.blue);
                if(!isFlashlightHPVisible)
                {
                    flashlightVisiblePoint = flashlightHitPoint;
                }
                else
                {
                    if(Vector3.Distance(playerPos, flashlightHitPoint) < Vector3.Distance(playerPos, flashlightVisiblePoint))
                    {
                        flashlightVisiblePoint = flashlightHitPoint;
                    }
                }
                isFlashlightHPVisible = true;
            }
        }

        return isFlashlightHPVisible;
    }
    private float remap(float iMin, float iMax, float oMin, float oMax, float value)
    {
        float val;
        val = Mathf.InverseLerp(iMin, iMax, value);
        return Mathf.Lerp(oMin, oMax, val);
    }
    private void isPlayerAttacked()
    {
        if (attackCooldown > 0.0f) 
        {
            attackCooldown -= Time.fixedDeltaTime;
            return;
        }
        float attackDistance = Vector3.Distance(player.transform.position, agent.transform.position);
        if (attackDistance < 1.7f)
        {
            //postProcessing.depthOfFieldEnabled = true;
            AudioManager.instance.playAudio("player_grunt" + Random.Range(1,3).ToString());
            attackCooldown = 1.0f;
            float hitStrength = 60.0f - remap(0.9f, 1.7f, 20.0f, 40.0f, attackDistance);
            Debug.Log("Player is attacked!");
            Debug.Log("HEALTH: " + GameManager.instance.health + ", ATTACK DISTANCE: " + attackDistance + ", HIT: -" + hitStrength + "HP");
            GameManager.instance.health -= hitStrength;
        }
    }
    private void playRandomSound()
    { 
        if(Vector3.Distance(player.transform.position, agent.transform.position) < 10.0f && !effectAudioSource.isPlaying)
        {
            AudioManager.instance.playAudio("zombie" + Random.Range(1, 5).ToString(), effectAudioSource);
        }
        int nextTime = Random.Range(3, 9);
        Invoke("playRandomSound", nextTime);
    }
    private void playAudioOnChaseBegin()
    {
        if(!effectAudioSource.isPlaying)
        {
            AudioManager.instance.playAudio("zombie" + Random.Range(1, 5).ToString(), effectAudioSource);
        }
    }
}
