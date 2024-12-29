using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    
    // Sets the maximum health value and initializes the slider
    public void setMaxHealth(int health) {
        slider.maxValue = health;
        slider.value = health;
    }

    // Updates the current health on the slider
    public void setHealth(int health) {
        slider.value = health;
    }
}
