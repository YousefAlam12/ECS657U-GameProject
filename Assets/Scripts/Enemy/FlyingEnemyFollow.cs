using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemyFollow : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent flyingEnemy;
    public float hoverHeight = 2.1f;  // Height above the ground to simulate flying
    public float aggroRange = 10.0f;

    void Start()
    {
        // Ensure the agent doesn't automatically adjust its Y position
        flyingEnemy.updateUpAxis = false;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= aggroRange)
        {
            ChasePlayer();
        }

        // Adjust the position to maintain hover height
        AdjustHeight();
    }

    void ChasePlayer()
    {
        flyingEnemy.SetDestination(player.position);
    }

    void AdjustHeight()
    {
        // Use a Raycast to find the ground height
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            Vector3 targetPosition = flyingEnemy.nextPosition; // Use NavMeshAgent's calculated position
            targetPosition.y = hit.point.y + hoverHeight; // Adjust Y position for hover height
            transform.position = targetPosition; // Apply the adjusted position
        }
    }
}