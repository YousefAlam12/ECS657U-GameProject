using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : MonoBehaviour
{
    public Transform player;             // Reference to the player
    public NavMeshAgent agent;           // NavMeshAgent for chasing and patrolling
    public float aggroRange = 10f;       // Distance within which the boss starts chasing
    public float jumpRange = 3f;         // Distance within which the boss performs the jump
    public float jumpHeight = 2f;        // How high the boss jumps
    public float jumpCooldown = 3f;      // Time between slam sequences
    public GameObject shockwavePrefab;   // Prefab for the shockwave
    public float upwardDuration = 1f;    // Time to reach the peak of the jump
    public float downwardDuration = 0.5f; // Time to return to the ground

    public float normalSpeed = 3.5f;     // Normal movement speed
    public float chaseSpeed = 5f;        // Increased speed when chasing the player
    public float patrolRadius = 5f;      // Radius for random patrol points
    public float patrolWaitTime = 2f;    // Wait time at each patrol point

    public int slamCount = 3;            // Number of slams in a sequence
    public float slamDecrement = 0.3f;

    private bool isJumping = false;      // Is the boss currently performing a jump?
    private bool isPatrolling = false;   // Is the boss currently patrolling?

    void Start()
    {
        // Ensure the agent doesn't automatically adjust its Y position
        agent.updateUpAxis = false;

        // Set the initial speed to normal speed
        agent.speed = normalSpeed;

        // Start patrolling
        StartCoroutine(Patrol());
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isJumping)
        {
            // If currently jumping, don't chase or patrol
            return;
        }

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

            // Optional: Add a brief delay between slams
            yield return new WaitForSeconds(0.5f);
        }

        // Resume the agent after the slam sequence
        agent.isStopped = false;

        // Wait for the cooldown before the next sequence
        yield return new WaitForSeconds(jumpCooldown);

        isJumping = false;
    }

    private IEnumerator Jump(float currentUpwardDuration, float currentDownwardDuration)
    {
        // Store the current position as the starting point for the jump
        Vector3 startPosition = transform.position;
        Vector3 jumpTarget = startPosition + Vector3.up * jumpHeight;

        // Smoothly move upwards
        float elapsedTime = 0f;
        while (elapsedTime < currentUpwardDuration)
        {
            transform.position = Vector3.Lerp(startPosition, jumpTarget, elapsedTime / currentUpwardDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the boss reaches the peak
        transform.position = jumpTarget;

        // Smoothly move downwards
        elapsedTime = 0f;
        while (elapsedTime < currentDownwardDuration)
        {
            transform.position = Vector3.Lerp(jumpTarget, startPosition, elapsedTime / currentDownwardDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the boss returns to the ground
        transform.position = startPosition;

        // Trigger the shockwave at the base of the boss
        if (shockwavePrefab != null)
        {
            // Calculate the position for the shockwave at the base of the boss
            Vector3 shockwavePosition = transform.position + Vector3.down * (transform.localScale.y / 2f);
            GameObject shockwave = Instantiate(shockwavePrefab, shockwavePosition, Quaternion.identity);

            // Set the shockwave scale to not expand on the Y-axis
            Vector3 shockwaveScale = shockwave.transform.localScale;
            shockwave.transform.localScale = new Vector3(shockwaveScale.x, 1f, shockwaveScale.z); // Set Y scale to 1
        }
    }


    private IEnumerator Patrol()
    {
        isPatrolling = true;

        while (true)
        {
            // Choose a random point within the patrol radius
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
            {
                // Move to the random point
                agent.SetDestination(hit.position);

                // Wait until the agent reaches the destination
                while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
                {
                    yield return null;
                }

                // Wait for some time at the patrol point
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