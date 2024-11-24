using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthAmount = 20; // Amount of health to add when picked up
    private HealthBar healthBar; // Reference to the HealthBar script

    void Start()
    {
        // Find the HealthBar in the scene
        healthBar = FindAnyObjectByType<HealthBar>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player")) // Ensure your player has the "Player" tag
        {
            // Increase the player's health and ensure it is an integer
            int newHealth = Mathf.Clamp((int)(healthBar.slider.value + healthAmount), 0, (int)healthBar.slider.maxValue);
            healthBar.setHealth(newHealth);

            // Optionally, destroy the pickup object after use
            Destroy(gameObject);
        }
    }
}