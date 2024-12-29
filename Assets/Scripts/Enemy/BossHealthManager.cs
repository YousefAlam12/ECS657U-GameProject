using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthManager : MonoBehaviour
{
    public EnemyHealth enemyHealth; // Reference to the EnemyHealth script
    public BossHealth healthBar; // Reference to the BossHealthBar script

    // to be spawned once boss is beat
    public GameObject treasure;

    // Flag to ensure treasure is spawned only once
    private bool treasureSpawned = false;

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
        else
        {
            healthBar.SetHealth(0);
        }

        // spawn treasure once boss dies
        if (healthBar.healthSlider.value <= 0 && !treasureSpawned)
        {
            Instantiate(treasure, new Vector3(-89.83105f, 15.01f, 85.44401f), Quaternion.Euler(0, 90.071f, 0));
            treasureSpawned = true;
        }
    }
}
