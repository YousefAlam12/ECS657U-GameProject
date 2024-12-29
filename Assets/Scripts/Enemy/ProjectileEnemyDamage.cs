using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyDamage : MonoBehaviour
{
    public int damageAmount = 1;
    private bool hasDealtDamage = false;

    void OnCollisionEnter(Collision other)
    {
        // Check if the projectile collides with the player
        if (other.collider.CompareTag("Player"))
        {
            if (hasDealtDamage) return;

            HealthManager playerHealth = FindAnyObjectByType<HealthManager>();
            if (playerHealth != null)
            {
                playerHealth.damagePlayer(damageAmount, Vector3.zero);
                hasDealtDamage = true; // Mark damage as dealt so projectile dosen't collide with target more than once
            }

            // Destroy the projectile after it hits the player
            Destroy(gameObject);
        }
    }
}