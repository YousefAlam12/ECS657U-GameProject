using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public List<GameObject> pickups;
    public int amount;
    public Vector3 spawnArea;

    public LayerMask spawnableLayer;  // Add a LayerMask to filter for the "Spawnable" layer

    // Procedurally generate content in the form of pickup items across selected range
    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Debug.Log(i);
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnArea.x, spawnArea.x),
                spawnArea.y,  // You can keep this for the fixed height
                Random.Range(-spawnArea.z, spawnArea.z)
            );

            // Offset this position by the Spawner's current position
            randomPosition += transform.position;

            // Check if the position is valid (on the "Spawnable" layer)
            if (IsSpawnable(randomPosition))
            {
                if (pickups.Count > 0)
                {
                    GameObject randomPickup = pickups[Random.Range(0, pickups.Count)];
                    Instantiate(randomPickup, randomPosition, Quaternion.identity);
                }
            }
            else
            {
                i--; // Retry this iteration if the position is not spawnable
            }
        }
    }

    // Function to check if the position is on the "Spawnable" layer
    bool IsSpawnable(Vector3 position)
    {
        // Cast a ray downward from the spawn position to check if it hits the "Spawnable" layer
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 100f, Vector3.down, out hit, Mathf.Infinity, spawnableLayer))
        {
            return true;
        }
        return false;
    }

    // Draws spawn area in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnArea * 2);
    }
}
