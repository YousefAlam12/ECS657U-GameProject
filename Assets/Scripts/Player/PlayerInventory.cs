using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfTreasure;
    public bool isPoweredup;
    public GameObject powerup;
    private PlayerController player;

    // variables for the Dash powerup
    public float dashDecay = 5f;
    public float dashPower = 20f;
    public float dashCooldown = 0.5f;
    private float nextDashTime = 0f;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    public void TreasureCollected()
    {
        NumberOfTreasure++;
    }

    // dash power allows player to dash in the direction that they are currently moving in
    public void dash()
    {
        Debug.Log("dashing");

        // checks cooldown
        if (Time.time >= nextDashTime) {
            if (player.moveInput != Vector2.zero) {
                // Calculate dash direction based on current movement direction
                player.dashDirection = (transform.forward * player.moveInput.y + transform.right * player.moveInput.x).normalized * dashPower;
            }
            else {
                // when not moving defualt dash will go forward
                player.dashDirection = (transform.forward * 1 + transform.right * player.moveInput.x).normalized * dashPower;
            }
            // Update next dash time
            nextDashTime = Time.time + dashCooldown;
        }
    }

    public void usePowerUp()
    {
        if (powerup.GetComponent<Dash>() != null) {
            dash();
        }

        // if (powerup.GetComponent<Shoot>() != null) {
                // add shooting logic in the shoot script 
                // if you need to add vars to player controller thats ok but dont do it unecessarily (also make sure you comment them if you do)
        // }
    }
}
