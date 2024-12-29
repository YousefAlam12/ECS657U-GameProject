using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretTreasure : MonoBehaviour
{
    // Adds secret treasure to global var and then dissapears
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.SecretTreasureCollected();
            gameObject.SetActive(false);
        }

    }
}
