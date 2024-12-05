using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyJumpDeath : MonoBehaviour
{
    public int damageToEnemy = 3; // Damage dealt to the enemy when jumped on
    public float bounceForce = 10f; // Force applied to the player for the bounce

    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        // reduce the enemy speed on easy mode
        if (MainMenuManager.isEasy())
        {
            agent.speed = agent.speed - 2;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // // Note: Used raycasting because player is kinematic. Dosen't rely on physics like velocity to determine if the player is directly above the enemy

            // // Cast a ray from the player's position downwards
            // RaycastHit hit;
            // Vector3 rayOrigin = other.transform.position;
            // Vector3 rayDirection = Vector3.down;

            // if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity))
            // {
            //     // Check if the ray hit this enemy object
            //     if (hit.collider.gameObject == gameObject)
            //     {
                    // Damage the enemy
                    EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(damageToEnemy);
                    }

                    // Bounce the player back
                    PlayerController playerController = other.GetComponent<PlayerController>();
                    if (playerController != null)
                    {
                        playerController.Bounce(bounceForce);
                    }
            //    }
        //    }
        }
    }
}
