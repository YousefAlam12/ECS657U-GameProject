using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthManager : MonoBehaviour
{
    public EnemyHealth enemyHealth; // Reference to the EnemyHealth script
    public BossHealth healthBar; // Reference to the BossHealthBar script

    void Start()
    {
        if (enemyHealth != null && healthBar != null)
        {
            // Initialize the health bar with the enemy's max health
            healthBar.SetMaxHealth(enemyHealth.maxHealth);
        }
    }

    void Update()
    {
        if (enemyHealth != null && healthBar != null)
        {
            // Update the health bar with the enemy's current health
            healthBar.SetHealth(enemyHealth.currentHealth);
        }
    }
}
