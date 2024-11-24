using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private PlayerInventory inventory;

    void Start()
    {
        inventory = FindAnyObjectByType<PlayerInventory>();
    }

    // Sets the current powerup in inventory to dash 
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            inventory.powerup = gameObject;
            inventory.isPoweredup = true;
            gameObject.SetActive(false);
        }
    }

}
