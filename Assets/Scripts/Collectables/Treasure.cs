using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    // Adds treasure to player inventory and then dissapears
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent < PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.TreasureCollected();
            gameObject.SetActive(false);
        }

    }
}
