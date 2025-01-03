using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3; // Set a default maximum health value
    public int currentHealth;
    public GameObject enemyAgent;

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health
        enemyAgent = transform.parent.gameObject;
    }

    // Call this method to reduce health
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took damage! Current health: " + currentHealth);
        PlayerSoundManager soundManager = UnityEngine.Object.FindAnyObjectByType<PlayerSoundManager>();
        if (soundManager != null)
        {
            soundManager.PlayDamageSound();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Handle enemy death
    private void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        
        // Destroy the enemy object
        Destroy(enemyAgent);
        Destroy(gameObject);
    }
}