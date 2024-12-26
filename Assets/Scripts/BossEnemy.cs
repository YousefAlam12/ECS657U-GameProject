using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : MonoBehaviour
{
    public Transform player;             
    public NavMeshAgent agent;           // NavMeshAgent for chasing and patrolling
    public float aggroRange = 10f;       // Distance which the boss starts chasing
    public float jumpRange = 3f;         // Distance which the boss performs the jump
    public float jumpHeight = 2f;        // How high the boss jumps
    public float jumpCooldown = 3f;      // Time between slam sequences
    public GameObject shockwavePrefab;
    public float upwardDuration = 1f;    // Time to reach the peak of the jump
    public float downwardDuration = 0.5f; // Time to return to the ground

    public float normalSpeed = 3.5f;     // Normal movement speed
    public float chaseSpeed = 5f;        // Increased speed when chasing the player
    public float patrolRadius = 5f;      // Radius for random patrol points
    public float patrolWaitTime = 2f;    // Wait time at each patrol point

    public int slamCount = 3;            // Number of slams in a sequence
    public float slamDecrement = 0.3f;

    private bool isJumping = false;      
    private bool isPatrolling = false;   
    private bool encounterStarted = false;

    public GameObject bossHealthBar;

    public float shockwaveHeight = 2f;
    public float shockwaveWidth = 0.4f;

    // enemy health script
    // public GameObject whaleModel;
    
    // to be spawned once boss is beat
    // public GameObject treasure;

    void Start()
    {
        agent.updateUpAxis = false;
        // whaleModel = GameObject.Find("WhaleBoss").transform.GetChild(0).gameObject;

        // Set the initial speed to normal speed
        agent.speed = normalSpeed;

        // Hide the health bar initially
        if (bossHealthBar != null)
        {
            bossHealthBar.SetActive(false);
        }

        // Stop the agent from moving initially
        agent.isStopped = true;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isJumping)
        {
            // If currently jumping, don't chase or patrol
            return;
        }

        if (!encounterStarted && distanceToPlayer <= aggroRange)
        {
            // Start the encounter when the player enters the aggro range
            StartEncounter();
        }

        if (encounterStarted)
        {
            if (distanceToPlayer <= jumpRange)
            {
                // Player is in range to perform the jump sequence
                StopPatrolling();
                StartCoroutine(SlamSequence());
            }
            else if (distanceToPlayer <= aggroRange)
            {
                // Chase the player if within aggro range
                StopPatrolling();
                SetChaseSpeed();
                ChasePlayer();
            }
            else
            {
                // Start patrolling if the player is out of range
                SetNormalSpeed();
                if (!isPatrolling)
                {
                    StartCoroutine(Patrol());
                }
            }
        }

        // if (whaleModel == null || whaleModel.GetComponent<EnemyHealth>().currentHealth <= 0)
        // {
        //     // Instantiate(treasure, new Vector3(-89.83105f, 15.01f, 85.44401f), Quaternion.identity);
        //     // Instantiate(treasure, new Vector3(-89.83105f, 15.01f, 85.44401f), Quaternion.Euler(0, 90.071f, 0));
        //     Destroy(gameObject);
        // }
    }

    private void StartEncounter()
    {
        encounterStarted = true;

        if (bossHealthBar != null)
        {
            bossHealthBar.SetActive(true);
        }

        // Allow the boss to start moving
        agent.isStopped = false;
        Debug.Log("Boss encounter started!");
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void SetChaseSpeed()
    {
        agent.speed = chaseSpeed;
    }

    private void SetNormalSpeed()
    {
        agent.speed = normalSpeed;
    }

    private IEnumerator SlamSequence()
    {
        isJumping = true;

        // Stop the agent while performing the slam sequence
        agent.isStopped = true;

        // Initialize the durations
        float currentUpwardDuration = upwardDuration;
        float currentDownwardDuration = downwardDuration;

        for (int i = 0; i < slamCount; i++)
        {
            // Perform a single slam with the current durations
            yield return StartCoroutine(Jump(currentUpwardDuration, currentDownwardDuration));

            // Decrease the durations for the next slam
            currentUpwardDuration = Mathf.Max(0.2f, currentUpwardDuration - slamDecrement); // Ensure upward duration doesn't go below 0.2 seconds
            currentDownwardDuration = Mathf.Max(0.1f, currentDownwardDuration - slamDecrement); // Ensure downward duration doesn't go below 0.1 seconds

            yield return new WaitForSeconds(0.5f);
        }

        agent.isStopped = false;
        yield return new WaitForSeconds(jumpCooldown);
        isJumping = false;
    }

    private IEnumerator Jump(float currentUpwardDuration, float currentDownwardDuration)
    {
        // Store the current position as the starting point for the jump
        Vector3 startPosition = transform.position;
        Vector3 jumpTarget = startPosition + Vector3.up * jumpHeight;

        // Move up
        float elapsedTime = 0f;
        while (elapsedTime < currentUpwardDuration)
        {
            transform.position = Vector3.Lerp(startPosition, jumpTarget, elapsedTime / currentUpwardDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = jumpTarget;
        elapsedTime = 0f;
        while (elapsedTime < currentDownwardDuration)
        {
            transform.position = Vector3.Lerp(jumpTarget, startPosition, elapsedTime / currentDownwardDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Boss returns to the ground
        transform.position = startPosition;

        // Trigger the shockwave at the base of the boss
        if (shockwavePrefab != null)
        {
            // Calculate the position for the shockwave
            Vector3 shockwavePosition = transform.position + Vector3.down * shockwaveHeight;
            GameObject shockwave = Instantiate(shockwavePrefab, shockwavePosition, Quaternion.identity);

            Vector3 shockwaveScale = shockwave.transform.localScale;
            shockwave.transform.localScale = new Vector3(shockwaveScale.x, shockwaveWidth, shockwaveScale.z); // Set Y scale to 1
        }
    }

    private IEnumerator Patrol()
    {
        isPatrolling = true;

        while (true)
        {
            // Choose a random point within the patrol radius to move to
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);

                while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(patrolWaitTime);
            }
        }
    }

    private void StopPatrolling()
    {
        isPatrolling = false;
        StopCoroutine(Patrol());
    }
}
