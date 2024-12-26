using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyJumpDeath : MonoBehaviour
{
    public int damageToEnemy = 3; // Damage dealt to the enemy when jumped on
    public float bounceForce = 10f; // Force applied to the player for the bounce
    public float pushBackDuration = 0.5f; // Duration of the push-back effect
    public GameObject enemy;
    public NavMeshAgent agent;

    private bool isPushedBack = false;
    private Vector3 pushBackDirection;
    private float pushBackTimer = 0;

    void Start()
    {
        enemy = transform.parent.gameObject;
        agent = enemy.GetComponent<NavMeshAgent>();

        // Reduce the enemy speed on easy mode
        if (MainMenuManager.isEasy())
        {
            agent.speed -= 2;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Damage the enemy
            EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageToEnemy);
                
                // increase speed when on standard
                if (!MainMenuManager.isEasy())
                {
                    agent.speed += 2;
                }
            }

            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.Bounce(1f);

            if (enemyHealth.currentHealth > 0)
            {
                // Bounce the player back
                // PlayerController playerController = other.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    Vector3 direction = (enemy.transform.position - other.transform.position).normalized;
                    pushBackDirection = direction * bounceForce;
                    isPushedBack = true;
                    pushBackTimer = pushBackDuration;

                    // trigger a player bounceback
                    // playerController.Bounce(1f);
                    // Vector3 d = other.transform.position - enemy.transform.position;
                    // playerController.Knockback(new Vector3(0.5f, 0.5f, 0.5f));
                }
            }

        }
    }

    void Update()
    {
        if (isPushedBack)
        {
            // Smoothly move the enemy back
            float deltaTime = Time.deltaTime;
            float moveDistance = (20f) * deltaTime;
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemy.transform.position + pushBackDirection, moveDistance);

            // Decrease the pushback timer
            pushBackTimer -= deltaTime;
            if (pushBackTimer <= 0)
            {
                isPushedBack = false; // Stop pushing the enemy back
            }
        }
    }
}
