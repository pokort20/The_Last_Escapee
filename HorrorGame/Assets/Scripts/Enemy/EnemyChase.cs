/// Main enemy class
/**
    This class handles the enemy states and movement. It uses
    the NavMesh agent, to which it passes the destination where
    to go. This destination is computed based on enemy's hearing,
    vision or flashlight's light.
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyChase : MonoBehaviour
{
    //Public variables defined in Unity inspector
    public NavMeshAgent agent;
    public Transform player;
    public GameObject patrolPoints;
    public float enemyFOV;
    public float agentSpeed;
    public float hearingRange;
    public AudioSource loopAudioSource;
    public AudioSource effectAudioSource;

    //Other variables
    private Vector3 flashlightVisiblePoint;
    private bool isPrevDestFromFlashlight;
    private float attackCooldown;
    private PostProcessing postProcessing;
    private Tutorial tutorial;
    private Animator animator;
    float attackDistance;
    
    //States
    private enum enemyStates : int
    {
        PATROL = 0,
        CHASING = 1,
        EXPLORE_AREA = 2
    }
    private enemyStates enemyState;

    //Init
    void Start()
    {
        postProcessing = PostProcessing.instance;
        tutorial = Tutorial.instance;
        flashlightVisiblePoint = Vector3.zero;
        isPrevDestFromFlashlight = false;
        enemyState = enemyStates.PATROL;
        agent.speed = agentSpeed;
        attackCooldown = 0.0f;
        animator = GetComponentInChildren<Animator>();
        EnemyAudio ea = new EnemyAudio(GetHashCode(), agent.velocity, agent.transform.position, loopAudioSource, effectAudioSource);
        AudioManager.instance.addEnemy(ea);
        Invoke("playRandomSound", Random.Range(5, 15));
    }

    //Update
    void Update()
    {
        switch (enemyState)
        {
            case enemyStates.PATROL:
                patrol();
                break;
            case enemyStates.CHASING:
                chase();
                break;
            case enemyStates.EXPLORE_AREA:
                explore();
                break;
            default:
                Debug.LogError("Enemy state unknown!");
                break;
        }
        AudioManager.instance.updateEnemy(GetHashCode(), agent.transform.position, agent.velocity);
        isPlayerAttacked();
    }

    //Other funcitons
    //Patrol function
    private void patrol()
    {
        animator.SetFloat("speed", 0.0f, 0.15f, Time.deltaTime);
        if (isPlayerVisible(agent.transform.position, player.position) || isPlayerHearable(agent.transform.position, player.position)
           || isStruckByFlashlight() || isFlashlightHitPointVisible(agent.transform.position, player.position))
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
        }
        if (!agent.hasPath)
        {
            //find new patrol point
            Transform selectedPatrolPoint = patrolPoints.transform.GetChild(Random.Range(0, patrolPoints.transform.childCount));
            agent.SetDestination(selectedPatrolPoint.position);
        }
    }
    //Chase function
    private void chase()
    {
        animator.SetFloat("speed", 1.0f, 0.15f, Time.deltaTime);
        if (isPlayerVisible(agent.transform.position, player.position) || isPlayerHearable(agent.transform.position, player.position) || isStruckByFlashlight())
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
    //Explore function
    private void explore()
    {
        animator.SetFloat("speed", 0.8f, 0.15f, Time.deltaTime);
        if (isPlayerVisible(agent.transform.position, player.position) || isPlayerHearable(agent.transform.position, player.position)
           || isStruckByFlashlight() || isFlashlightHitPointVisible(agent.transform.position, player.position))
        {
            Debug.Log("Player spotted while exploring area, switching to CHASING!");
            enemyState = enemyStates.CHASING;
            agent.speed = agentSpeed * 2.0f;
            playAudioOnChaseBegin();
            chase();
            return;
        }
        if (!agent.hasPath)
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
        float visibilityDistance = remap(0.0f, 70.0f, 2.0f, 18.0f, playerVisibility);
        //Debug.Log("visibility distance: " + visibilityDistance);
        
        Vector3 eyePos = agentPos;
        eyePos.y += 1.2f;
        Vector3 dirToPlayer = playerPos - eyePos;
        float angle = Vector3.Angle(dirToPlayer, agent.transform.forward);
        if (Physics.Raycast(eyePos, playerPos - eyePos, out raycastHit, visibilityDistance))
        {
            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Player") && angle < enemyFOV * 0.5f)
            {
                return true;
            }
        }
        return false;
    }
    private bool isPlayerHearable(Vector3 agentPos, Vector3 playerPos)
    {
        float moveVecMag = player.GetComponent<PlayerMovement>().getPlayerMoveVec().magnitude;
        float noiseRange = moveVecMag * 0.5f;
        noiseRange = Mathf.Pow(noiseRange, 2.45f);
        float dist = Vector3.Distance(agentPos, playerPos);
        RaycastHit raycastHit;
        Vector3 earPos = agentPos;
        earPos.y += 1.2f;
        int layerMask = LayerMask.GetMask("Walls");

        if (dist < hearingRange + noiseRange)
        {
            if (Physics.Raycast(earPos, playerPos - earPos, out raycastHit, hearingRange + noiseRange, layerMask: layerMask))
            {
                if(2*dist < hearingRange + noiseRange)
                {
                    return true;
                }
                return false;
            }
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
            //No flashlight hit points, either disabled or not coliding with anything.
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
    private bool isStruckByFlashlight()
    {
        List<int> enemiesStruckByFlashlight = FindObjectOfType<EnemyUtilities>().enemiesStruckByFlashlight;
        if (enemiesStruckByFlashlight == null || enemiesStruckByFlashlight.Count == 0)
        {
            return false;
        }

        foreach(int i in enemiesStruckByFlashlight)
        {
            if(i == gameObject.GetHashCode())
            {
                //Flashlight shines on enemy
                return true;
            }
        }
        return false;
    }
    private float remap(float iMin, float iMax, float oMin, float oMax, float value)
    {
        float val;
        val = Mathf.InverseLerp(iMin, iMax, value);
        return Mathf.Lerp(oMin, oMax, val);
    }
    //Attack functions
    private void isPlayerAttacked()
    {
        if (attackCooldown > 0.0f) 
        {
            attackCooldown -= Time.fixedDeltaTime;
            return;
        }
        attackDistance = Vector3.Distance(player.transform.position, agent.transform.position);
        if (attackDistance < 1.9f)
        {
            RaycastHit raycastHit;
            Physics.Raycast(agent.transform.position + new Vector3(0.0f, 1.0f, 0.0f), player.transform.position - (agent.transform.position + new Vector3(0.0f, 1.0f, 0.0f)), maxDistance: 5.0f, hitInfo: out raycastHit);
            if(raycastHit.collider != null)
            {
                Debug.DrawRay(agent.transform.position + new Vector3(0.0f, 1.0f, 0.0f), raycastHit.point - (agent.transform.position + new Vector3(0.0f, 1.0f, 0.0f)), Color.red);
            }
            if(raycastHit.collider != null && raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                attackCooldown = 1.0f;
                animator.SetTrigger("attack");
                Invoke("attackPlayer", 0.2f);
            }
        }
    }
    private void attackPlayer()
    {
        AudioManager.instance.playAudio("player_grunt1");
        float hitStrength = 60.0f - remap(0.9f, 1.7f, 20.0f, 40.0f, attackDistance);
        Debug.Log("Player is attacked!");
        Debug.Log("HEALTH: " + GameManager.instance.health + ", ATTACK DISTANCE: " + attackDistance + ", HIT: -" + hitStrength + "HP");
        GameManager.instance.health -= hitStrength;
        if (Inventory.instance.hasItem(typeof(MedkitItem)) > 0 && GameManager.instance.health <= GameManager.instance._maxHealth * 0.5f)
        {
            if (tutorial != null)
            {
                tutorial.showHelp("medkit");
            }
        }
        
    }
    //Audio handlers
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
