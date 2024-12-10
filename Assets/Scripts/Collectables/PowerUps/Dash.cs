using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    private PlayerInventory inventory;
    public Image icon;
    public Sprite powerIcon;

    void Start()
    {
        inventory = FindAnyObjectByType<PlayerInventory>();
        // icon = GameObject.FindGameObjectWithTag("Icon").GetComponent<Image>();
        // inventory.icon = icon;
        // icon.gameObject.SetActive(false); 
    }

    // Sets the current powerup in inventory to dash 
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            inventory.powerup = gameObject;
            inventory.isPoweredup = true;
            gameObject.SetActive(false);

            // setting the icon to the current powerup
            inventory.icon.sprite = powerIcon;
            inventory.icon.gameObject.SetActive(true);
        }
    }

}
