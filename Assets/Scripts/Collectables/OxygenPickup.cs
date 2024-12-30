using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenPickup : MonoBehaviour
{
    public float oxygenAmount = 11f; 
    private OxygenBar oxygenBar; 
    public float respawnTime = 5f;
    private MeshRenderer meshRenderer;
    private Collider collider;

    void Start()
    {
        // Find the OxygenBar in the scene
        oxygenBar = FindAnyObjectByType<OxygenBar>();

        // Cache references to components
        meshRenderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure your player has the "Player" tag
        {
            Debug.Log("Oxygen pickup collected by player.");

            // Increase the oxygen level
            oxygenBar.oxygen += oxygenAmount;

            // Clamp the oxygen value to ensure it doesn't exceed the maximum
            oxygenBar.oxygen = Mathf.Clamp(oxygenBar.oxygen, 0, oxygenBar.maxOxygen);

            // Start the respawn coroutine
            StartCoroutine(Respawn());
        }
    }

    // respawns the pickup after set time
    private IEnumerator Respawn()
    {
        // Hide the object by disabling its mesh and collider
        meshRenderer.enabled = false;
        collider.enabled = false;

        // Wait for the specified respawn time
        yield return new WaitForSeconds(respawnTime);

        // Show the object again
        meshRenderer.enabled = true;
        collider.enabled = true;
    }
}
