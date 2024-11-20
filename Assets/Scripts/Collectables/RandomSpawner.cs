using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public List<GameObject> pickups;
    public int amount;
    public Vector3 spawnArea;
    

    // Procedurally generate content in the form of pickup items across selected range
    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Debug.Log(i);
            // Vector3 randomPosition = new Vector3(Random.Range(-10, 11), 5, Random.Range(-10, 11));
            Vector3 randomPosition = new Vector3(Random.Range(-spawnArea.x, spawnArea.x), spawnArea.y, Random.Range(-spawnArea.z, spawnArea.z));

            if (pickups.Count > 0)
            {
                GameObject randomPickup = pickups[Random.Range(0, pickups.Count)];
                Instantiate(randomPickup, randomPosition, Quaternion.identity);
            }
        }
    }

    // Draws spawn area in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnArea * 2);
    }
}

