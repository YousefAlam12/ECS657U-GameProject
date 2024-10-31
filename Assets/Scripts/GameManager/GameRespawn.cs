using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRespawn : MonoBehaviour
{
    public Vector3 respawnPoint;  // player respawn point
    public float threshold;  // death boundary
    private HealthManager health;

    private void Start()
    {
        // set respawn point
        respawnPoint = transform.position;
        health = FindAnyObjectByType<HealthManager>();
    }

    // Checks to see if player fell off the stage
    void FixedUpdate()
    {
        if (transform.position.y < threshold)
        {
            // Debug.Log("Player fell below threshold! Respawning at: " + respawnPoint);
            transform.position = respawnPoint;
            health.damagePlayer(2, new Vector3(0f,0f,0f));
        }
    }

    public void UpdateRespawnPoint(Vector3 newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
        // Debug.Log("Respawn point updated to: " + respawnPoint);
    }
}
