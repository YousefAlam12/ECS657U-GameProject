using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupProjectile : MonoBehaviour
{
    private bool isThrown = false;
    private float throwTimeout = 5f; // Time reset the state
    private float throwTimer;
    public float threshold = -20;

    public void SetThrownState(bool state)
    {
        isThrown = state;
        if (state)
        {
            throwTimer = throwTimeout; // Reset the timer when thrown
        }
    }

    private void Update()
    {
        // Countdown timer to reset the thrown state if it doesn't hit anything
        if (isThrown)
        {
            throwTimer -= Time.deltaTime;
            if (throwTimer <= 0)
            {
                ResetThrownState();
            }
        }

        // if object falls below threshold it will be destroyed
        if (transform.position.y < threshold)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the thrown pickup hits an enemy
        if (isThrown && collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Thrown pickup hit an enemy!");
            Destroy(collision.gameObject); // Destroy the enemy
            Destroy(gameObject); // Destroy the pickup object
        }
        else
        {
            // If it hits anything else, reset the thrown state
            ResetThrownState();
        }
    }

    private void ResetThrownState()
    {
        isThrown = false;
        //Debug.Log("Thrown state reset.");
    }
}