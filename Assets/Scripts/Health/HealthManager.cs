using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public HealthBar healthBar;

    public int maxHealth = 5;
    public int currentHealth;

    public PlayerController thePlayer;

    // Setting up health and linking it to the healthbar
    void Start() {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
        thePlayer = FindAnyObjectByType<PlayerController>();
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
    }

    void Update() {
        // Testing
        // if(Input.GetKeyDown(KeyCode.L)) {
        //     damagePlayer(1);
        // }
    }

}
