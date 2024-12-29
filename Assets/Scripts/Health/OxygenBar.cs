using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    public Slider slider;
    public float oxygen;
    public float maxOxygen = 200f;
    public HealthManager health;

    // Sets the o2 bar in the UI to the max o2
    void Start()
    {
        if (MainMenuManager.isEasy())
        {
            maxOxygen = 300f;            
        }
        slider.maxValue = maxOxygen;
        oxygen = maxOxygen;
        health = FindAnyObjectByType<HealthManager>();
    }

    // Depletes the o2 bar and kills player if it drops to 0
    void Update()
    {
        slider.value = oxygen;

        // o2 constantly depletes during game
        oxygen -= 1f * Time.deltaTime;

        // kills player when o2 bar is 0
        if (oxygen <= 0 && health.currentHealth > 0) {
            health.damagePlayer(100, new Vector3(0f,0f,0f));
        }

    }
}
