using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public HealthBar healthBar;

    public int maxHealth = 5;
    public int currentHealth;

    public PlayerController thePlayer;
    public PlayerInventory inventory;

    // Setting up health and linking it to the healthbar
    void Start() {
        if (MainMenuManager.isEasy())
        {
            maxHealth = 10;
        }

        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
        thePlayer = FindAnyObjectByType<PlayerController>();
        inventory = FindAnyObjectByType<PlayerInventory>();
        healthBar = FindAnyObjectByType<HealthBar>();
    }

    // Damages the player reflects on the healthbar
    public void damagePlayer(int damage, Vector3 direction) {
        currentHealth -= damage;
        
        if (direction != Vector3.zero)
        {
            thePlayer.Knockback(direction);
        }
            
        if(currentHealth <= 0) {
            currentHealth = 0;
        }
        healthBar.setHealth(currentHealth);

        PlayerSoundManager soundManager = UnityEngine.Object.FindAnyObjectByType<PlayerSoundManager>();
        if (soundManager != null)
        {
            soundManager.PlayLoseSound();
        }

        // lose powerup on hit and reflect onto UI
        inventory.powerup = null;
        inventory.isPoweredup = false;
        if (inventory.icon)
        {
            inventory.icon.gameObject.SetActive(false);
        }
    }
}
