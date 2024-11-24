using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthAmount = 20; 
    public bool enableRespawn = true; 
    public float respawnTime = 5f; 

    private HealthManager healthManager; 
    private MeshRenderer meshRenderer; 
    private Collider collider;

    void Start()
    {
        // Find the HealthManager in the scene
        healthManager = FindAnyObjectByType<HealthManager>();

        // Cache references to components
        meshRenderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Increase the player's health through the HealthManager
            healthManager.currentHealth = Mathf.Clamp(healthManager.currentHealth + healthAmount,0,healthManager.maxHealth);

            // Update the health bar to reflect the new health
            healthManager.healthBar.setHealth(healthManager.currentHealth);

            // Handle the pickup based on the respawn setting
            if (enableRespawn)
            {
                StartCoroutine(Respawn());
            }
            else
            {
                // Destroy the pickup if respawn is disabled
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator Respawn()
    {
        // Hide the object by disabling its mesh and collider
        meshRenderer.enabled = false;
        collider.enabled = false;

        // Wait for the respawn time
        yield return new WaitForSeconds(respawnTime);

        // Show the object again
        meshRenderer.enabled = true;
        collider.enabled = true;
    }
}
