using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    private PlayerInventory inventory;
    public Sprite powerIcon;

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
            PlayerSoundManager soundManager = UnityEngine.Object.FindAnyObjectByType<PlayerSoundManager>();
            if (soundManager != null)
            {
                soundManager.PlayPowerUpSound();
            }
            gameObject.SetActive(false);

            // setting the icon to the current powerup
            inventory.icon.sprite = powerIcon;
            inventory.icon.gameObject.SetActive(true);
        }
    }

}
