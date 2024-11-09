using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    public int damageAmount = 1;
    private bool hasDealtDamage = false;

    void OnCollisionEnter(Collision other)
    {
        // Check if the projectile collides with the player
        if (other.collider.CompareTag("Player"))
        {
            if (hasDealtDamage) return;
            //Debug.Log("Projectile hit the player");

            HealthManager playerHealth = FindAnyObjectByType<HealthManager>();
            if (playerHealth != null)
            {
                playerHealth.damagePlayer(damageAmount, Vector3.zero);
                // nextDamageTime = Time.time + damageCooldown; // Set the next time the enemy can deal damage
                //Debug.Log("Damage applied to player");
                hasDealtDamage = true; // Mark damage as dealt so projectile dosen't collide with target more than once
            }

            // Destroy the projectile after it hits the player
            Destroy(gameObject);
        }
    }
}