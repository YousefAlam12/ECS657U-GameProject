using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform player;
    public float aggroRange = 10.0f;

    void Start()
    {
        
    }

    // Checks to see if player is wthin range of the enemy's attacking area
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= aggroRange) // will chase player if it within range
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        enemy.SetDestination(player.position);
    }
}
