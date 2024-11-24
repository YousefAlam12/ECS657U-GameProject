using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenPickup : MonoBehaviour
{
    public float oxygenAmount = 10f; // Amount of oxygen to add when picked up
    private OxygenBar oxygenBar; // Reference to the OxygenBar script

    void Start()
    {
        // Find the OxygenBar in the scene
        oxygenBar = FindAnyObjectByType<OxygenBar>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player")) // Ensure your player has the "Player" tag
        {
            // Increase the oxygen level
            oxygenBar.oxygen += oxygenAmount;

            // Clamp the oxygen value to ensure it doesn't exceed the maximum
            oxygenBar.oxygen = Mathf.Clamp(oxygenBar.oxygen, 0, oxygenBar.maxOxygen);

            // Optionally, destroy the pickup object after use
            Destroy(gameObject);
        }
    }
}