using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    public Slider slider;
    public float oxygen;
    public float maxOxygen = 100f;
    public HealthManager health;

    void Start()
    {
        slider.maxValue = maxOxygen;
        oxygen = maxOxygen;
        health = FindAnyObjectByType<HealthManager>();
    }

    void Update()
    {
        slider.value = oxygen;

        // o2 constantly depletes during game
        oxygen -= 1 * Time.deltaTime;

        if (oxygen <= 0) {
            health.damagePlayer(100, new Vector3(0f,0f,0f));
        }

    }
}
