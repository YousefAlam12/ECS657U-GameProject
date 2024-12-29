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

    // Powerup variables
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

    // Returns SecretTreasure count
    public int getSecretTreasure()
    {
        return SecretTreasure;
    }

    // sets the powerup ability to be used by the player
    public void usePowerUp()
    {
        if (powerup.GetComponent<Dash>() != null) {
            player.dash();
        }
        else if (powerup.GetComponent<Shoot>() != null)
        {
            powerup.GetComponent<Shoot>().Shooting();
        }
    }
}
