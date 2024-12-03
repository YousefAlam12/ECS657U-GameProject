using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public List<GameObject> pickups;
    public int amount;
    public Vector3 spawnArea;

    public LayerMask spawnableLayer;  // Add a LayerMask to filter for the "Spawnable" layer
    public float spreadFactor = 1f;  // Factor to control the spread of the objects (higher means more spread out)
    public bool allowSpawnInColliders = false;  // Control whether objects can spawn inside existing colliders
    public int maxRetries = 5;  // Maximum retries to spawn each object

    private int totalSpawned = 0;  // Total number of objects successfully spawned
    private int totalRetries = 0;  // Total number of retries across all spawns

    // Procedurally generate content in the form of pickup items across selected range
    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            bool objectSpawned = false;
            int retries = 0;

            while (!objectSpawned && retries < maxRetries) // Retry up to 'maxRetries' times for each object
            {
                retries++;

                // Randomize position within the spawn area, with some spread factor to ensure they aren't clamped together
                Vector3 randomPosition = new Vector3(
                    Random.Range(-spawnArea.x, spawnArea.x),
                    Random.Range(-spawnArea.y, spawnArea.y),
                    Random.Range(-spawnArea.z, spawnArea.z)
                );

                // Offset this position by the Spawner's current position
                randomPosition += transform.position;

                // Apply spread factor: adding random offset based on the spread factor
                randomPosition += new Vector3(
                    Random.Range(-spreadFactor, spreadFactor),
                    Random.Range(-spreadFactor, spreadFactor),
                    Random.Range(-spreadFactor, spreadFactor)
                );

                // Check if the position is valid (on the "Spawnable" layer and not colliding with other objects)
                if (IsSpawnable(randomPosition))
                {
                    if (pickups.Count > 0)
                    {
                        GameObject randomPickup = pickups[Random.Range(0, pickups.Count)];
                        Instantiate(randomPickup, randomPosition, Quaternion.identity);
                        objectSpawned = true; // Mark the object as successfully spawned
                        totalSpawned++;  // Increment the total spawned counter
                    }
                }
                else
                {
                    totalRetries++;  // Increment the total retries counter
                }
            }

            if (!objectSpawned)
            {
                Debug.LogWarning($"Failed to spawn object after {maxRetries} attempts.");
            }
        }

        // After all spawning, log the summary
        Debug.Log($"Spawn complete! {totalSpawned} objects spawned with {totalRetries} total retries.");
    }

    // Function to check if the position is on the "Spawnable" layer and not colliding with other objects (unless allowed)
    bool IsSpawnable(Vector3 position)
    {
        // Cast a ray downward from the spawn position to check if it hits the "Spawnable" layer
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 100f, Vector3.down, out hit, Mathf.Infinity, spawnableLayer))
        {
            // If spawning inside colliders is not allowed, use Physics.CheckSphere to check for overlapping colliders
            if (!allowSpawnInColliders && Physics.CheckSphere(position, spreadFactor, spawnableLayer))
            {
                return false;  // Return false if there's already something in the way
            }
            return true;  // Position is valid for spawning
        }
        return false;  // No valid position on the "Spawnable" layer
    }

    // Draws spawn area in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnArea * 2);
    }
}
