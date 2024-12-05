using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public Transform player;
    public float shootingRange = 20.0f;
    public GameObject enemyProjectile;
    public Transform spawnPoint;
    public float projectileSpeed = 30f;
    [SerializeField] private float reloadTime = 3f;
    private float shootCooldown;
    private bool isFlyingEnemy; // Flag to check if this is a flying enemy
    private FlyingEnemyFollow flyingEnemyFollow;

    void Start()
    {
        shootCooldown = reloadTime;

        // Get the FlyingEnemyFollow component from the same GameObject
        flyingEnemyFollow = GetComponent<FlyingEnemyFollow>();

        if (flyingEnemyFollow != null)
        {
            isFlyingEnemy = true; // Confirm this is a flying enemy if the component exists
        }
        else
        {
            isFlyingEnemy = false;
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if(isFlyingEnemy)
        {
            AdjustSpawnPointPosition();
        }

        // Only shoot if the player is within shooting range and in sight
        if (distanceToPlayer <= shootingRange && IsPlayerInSight())
        {
            //Debug.Log("Player is in sight and within range. Shooting...");
            ShootAtPlayer();
        }

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
                //Debug.Log("Player is in line of sight");
                return true; // Player is in sight
            }
        }

        //Debug.Log("Player is not in line of sight");
        return false; // Player is not in sight
    }

    void AdjustSpawnPointPosition()
    {
        // Ensure the spawn point is correctly positioned in front of the enemy
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        spawnPoint.position = transform.position + directionToPlayer * 0.55f;
        spawnPoint.position = new Vector3(spawnPoint.position.x, flyingEnemyFollow.hoverHeight, spawnPoint.position.z); // Adjust the Y position to match hover height
    }


    void ShootAtPlayer()
    {
        shootCooldown -= Time.deltaTime;
        if (shootCooldown > 0) return;
        shootCooldown = reloadTime;

        GameObject projectileObj = Instantiate(enemyProjectile, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        Rigidbody projectileRig = projectileObj.GetComponent<Rigidbody>();

        // Calculate direction to player and apply impulse force
        Vector3 directionToPlayer = (player.position - spawnPoint.position).normalized;
        projectileRig.AddForce(directionToPlayer * projectileSpeed, ForceMode.Impulse);

        // Destroy projectile after 5 seconds
        Destroy(projectileObj, 5f);
    }
}