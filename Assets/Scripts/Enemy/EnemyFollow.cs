using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform player;
    public float aggroRange = 10.0f;

    [SerializeField] private float timer = 5;
    private float projectileTime;

    public GameObject enemyProjectile;
    public Transform spawnPoint;
    public float projectileSpeed = 20;

    // Start is called before the first frame update
    void Start()
    {
        projectileTime = timer;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= aggroRange) // will chase player if it within range
        {
            ChasePlayer();
        }
        // Only shoot if player is within aggro range and in sight
        if (distanceToPlayer <= aggroRange && IsPlayerInSight())
        {
            ShootAtPlayer();
        }
    }

    void ChasePlayer()
    {
        enemy.SetDestination(player.position);
    }

    bool IsPlayerInSight()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Raycast from enemy position towards player
        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, distanceToPlayer))
        {
            // Check if the raycast hit the player
            if (hit.transform == player)
            {
                return true; // Player is in sight
            }
        }

        return false; // Player is not in sight
    }

    void ShootAtPlayer()
    {
        projectileTime -= Time.deltaTime;
        if (projectileTime > 0) return;
        projectileTime = timer;
        GameObject projectileObj = Instantiate(enemyProjectile, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        Rigidbody projectileRig = projectileObj.GetComponent<Rigidbody>();
        projectileRig.AddForce(projectileRig.transform.forward * projectileSpeed);
        Destroy(projectileObj, 5f);
    }
}
