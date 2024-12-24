using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public Slider healthSlider; // Reference to the Slider UI

    // Initialize the health bar with the max health value
    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    // Update the current health value on the health bar
    public void SetHealth(int currentHealth)
    {
        healthSlider.value = currentHealth;
    }
}
