using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public List<GameObject> pickups;
    public int amount;
    public Vector3 spawnArea;

    public LayerMask spawnableLayer;  // Add a LayerMask to filter for the "Spawnable" layer
    public float spreadFactor = 0.5f;  // Factor to control the spread of the objects (higher means more spread out)
    public bool allowSpawnInColliders = false;  // Control whether objects can spawn inside existing colliders
    public int maxRetries = 5;  // Maximum retries to spawn each object

    private int totalSpawned = 0;  // Total number of objects successfully spawned
    private int totalRetries = 0;  // Total number of retries across all spawns

    public GameObject collectables; // Var for the COLLECTABLES folder/gameobject in scene hierarchy

    // Procedurally generate content in the form of pickup items across selected range
    void Start()
    {
        collectables = GameObject.Find("COLLECTABLES");

        for (int i = 0; i < amount; i++)
        {
            bool objectSpawned = false;
            int retries = 0;

            while (!objectSpawned && retries < maxRetries) // Retry up to 'maxRetries' times for each object
            {
                retries++;

                // Randomize position within the spawn area, with a controlled spread factor to prevent out-of-bounds spawning
                Vector3 randomPosition = new Vector3(
                    Mathf.Clamp(Random.Range(-spawnArea.x, spawnArea.x), -spawnArea.x, spawnArea.x),  // Clamp to spawn area limits
                    Mathf.Clamp(Random.Range(-spawnArea.y, spawnArea.y), -spawnArea.y, spawnArea.y),  // Clamp to spawn area limits
                    Mathf.Clamp(Random.Range(-spawnArea.z, spawnArea.z), -spawnArea.z, spawnArea.z)   // Clamp to spawn area limits
                );

                // Offset this position by the Spawner's current position
                randomPosition += transform.position;

                // Apply spread factor: adding random offset based on the spread factor but ensuring it stays within bounds
                randomPosition += new Vector3(
                    Mathf.Clamp(Random.Range(-spreadFactor, spreadFactor), -spawnArea.x, spawnArea.x),
                    Mathf.Clamp(Random.Range(-spreadFactor, spreadFactor), -spawnArea.y, spawnArea.y),
                    Mathf.Clamp(Random.Range(-spreadFactor, spreadFactor), -spawnArea.z, spawnArea.z)
                );

                // Check if the position is valid (on the "Spawnable" layer and not colliding with other objects)
                if (IsSpawnable(randomPosition))
                {
                    if (pickups.Count > 0)
                    {
                        GameObject randomPickup = pickups[Random.Range(0, pickups.Count)];
                        GameObject spawnedObject = Instantiate(randomPickup, randomPosition, Quaternion.identity);
                        spawnedObject.transform.SetParent(collectables.transform); // sets new obj as child of the COLLECTABLES folder/gameobject
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
    public float spawnHeightOffset = 1f;  // Adjustable height offset for collision avoidance

    bool IsSpawnable(Vector3 position)
    {
        // Cast a ray downward from the spawn position to check if it hits the "Spawnable" layer
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 100f, Vector3.down, out hit, Mathf.Infinity, spawnableLayer))
        {
            // Adjust position height if spawning inside colliders is not allowed
            if (!allowSpawnInColliders)
            {
                // Check for overlapping colliders
                if (Physics.CheckSphere(position, spreadFactor, spawnableLayer))
                {
                    position.y += spawnHeightOffset;  // Apply the height offset
                                                      // Re-check after adjustment
                    if (Physics.CheckSphere(position, spreadFactor, spawnableLayer))
                    {
                        return false;  // Still colliding after adjustment
                    }
                }
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
