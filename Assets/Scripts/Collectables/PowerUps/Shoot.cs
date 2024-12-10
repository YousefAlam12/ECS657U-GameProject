using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    private PlayerInventory inventory;
    public Image icon;
    public Sprite powerIcon;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 30f;
    [SerializeField] private float shootCooldown = 0.5f;
    private float nextShootTime = 0f;

    void Start()
    {
        inventory = FindAnyObjectByType<PlayerInventory>();

        // icon = GameObject.FindGameObjectWithTag("Icon").GetComponent<Image>();
        // inventory.icon = icon;
        // icon.gameObject.SetActive(false); 
    }

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

    public void Shooting()
    {
        // Check if enough time has passed for the next shot
        if (Time.time >= nextShootTime)
        {
            // Debug.Log("Shooting"); // Log to confirm shooting occurs

            GameObject projectileObj = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation) as GameObject;
            Rigidbody projectileRig = projectileObj.GetComponent<Rigidbody>();

            if (projectileRig != null)
            {
                projectileRig.velocity = firePoint.forward * bulletSpeed;
            }

            // Update the cooldown timer
            nextShootTime = Time.time + shootCooldown;

            // Destroy projectile after 3 seconds
            Destroy(projectileObj, 3f);
        }
    }

}
