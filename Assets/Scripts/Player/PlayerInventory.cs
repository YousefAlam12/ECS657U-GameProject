using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerInventory : MonoBehaviour
{
    // Treasure varibales 
    public int NumberOfTreasure;
    public static int SecretTreasure;
    public TextMeshProUGUI treasureTxt;
    public int totalTreasure;

    public bool isPoweredup;
    public GameObject powerup;
    private PlayerController player;

    // icons for powerups
    public Image icon;

    void Start()
    {
        player = GetComponent<PlayerController>();
        treasureTxt.text = NumberOfTreasure + "/" + totalTreasure;
        icon = GameObject.FindGameObjectWithTag("Icon").GetComponent<Image>();
        icon.gameObject.SetActive(false); 
    }

    // adds to NumberOfTreasure and reflects across the UI
    public void TreasureCollected()
    {
        NumberOfTreasure++;
        treasureTxt.text = NumberOfTreasure + "/" + totalTreasure;
    }

    // adds to the SecretTreasure count
    public void SecretTreasureCollected()
    {
        SecretTreasure++;
    }

    public int getSecretTreasure()
    {
        return SecretTreasure;
    }

    // dash power allows player to dash in the direction that they are currently moving in
    // public void dash()
    // {
    //     Debug.Log("dashing");

    //     // checks cooldown
    //     if (Time.time >= nextDashTime) {
    //         if (player.moveInput != Vector2.zero) {
    //             // Calculate dash direction based on current movement direction
    //             player.dashDirection = (transform.forward * player.moveInput.y + transform.right * player.moveInput.x).normalized * dashPower;
    //         }
    //         else {
    //             // when not moving defualt dash will go forward
    //             player.dashDirection = (transform.forward * 1 + transform.right * player.moveInput.x).normalized * dashPower;
    //         }
    //         // Update next dash time
    //         nextDashTime = Time.time + dashCooldown;
    //     }
    // }

    // sets the powerup ability to be used by the player
    public void usePowerUp()
    {
        if (powerup.GetComponent<Dash>() != null) {
            // dash();
            player.dash();
        }
        else if (powerup.GetComponent<Shoot>() != null)
        {
            powerup.GetComponent<Shoot>().Shooting();
        }
    }
}
