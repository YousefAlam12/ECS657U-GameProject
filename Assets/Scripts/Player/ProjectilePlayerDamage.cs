using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePlayerDamage : MonoBehaviour
{
    public int damageAmount = 1;
    //private bool hasDealtDamage = false;

    void OnCollisionEnter(Collision other)
    {
        // Check if the projectile collides with the enemy
        if (other.collider.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }

            // Destroy the projectile after it hits
            Destroy(gameObject);
        }
    }
}
