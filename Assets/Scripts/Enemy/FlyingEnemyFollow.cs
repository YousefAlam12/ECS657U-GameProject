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
        // Move towards the player using NavMesh (X and Z axis only)
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= aggroRange) // will chase player if it within range
        {
            ChasePlayer();
        }

        // Use a Raycast to determine the current ground height below the enemy
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            // Set the Y position to hoverHeight above the ground
            Vector3 pos = transform.position;
            pos.y = hit.point.y + hoverHeight;
            transform.position = pos;
        }
    }

    void ChasePlayer()
    {
        flyingEnemy.SetDestination(player.position);
    }
}
