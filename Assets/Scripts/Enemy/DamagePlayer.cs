using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int damageAmount = 1;
    public float damageCooldown = 1.5f; // Cooldown in seconds
    private float nextDamageTime = 0f;
    public PlayerController player;

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
    }

    void Update()
    {
        
    }

    // On collision the direction of the knockback is calculated and set whilst reducing health of player
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            Vector3 hitDirection = other.transform.position - transform.position;
            hitDirection = hitDirection.normalized;

            HealthManager playerHealth = FindAnyObjectByType<HealthManager>();
            if (playerHealth != null)
            {
                playerHealth.damagePlayer(damageAmount, hitDirection*2);
                // nextDamageTime = Time.time + damageCooldown; // Set the next time the enemy can deal damage
            }
        }
    }
}
