using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    private PlayerInventory inventory;
    public Sprite powerIcon;
    public float respawnTime = 10f;
    public bool canRespawn = true;

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

            // If the power-up can respawn, invoke respawn logic
            if (canRespawn)
            {
                Invoke("RespawnPowerUp", respawnTime);
            }
        }
    }

     // This method is called after the respawnTime to reactivate the power-up
    void RespawnPowerUp()
    {
        gameObject.SetActive(true);
    }

}
