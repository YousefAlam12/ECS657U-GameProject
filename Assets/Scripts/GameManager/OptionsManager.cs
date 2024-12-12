using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsManager : MonoBehaviour
{
    // global var to allow camera to access its value
    public static float playerSens = 0.1f;
    public TextMeshProUGUI txt;
    public Slider slider; 

    // set the default values for the slider and text
    public void Start()
    {
        txt.text = playerSens.ToString();
        slider.value = playerSens;
    }

    // change the player sens dependent on slider position
    public void setSensitivity(float sens)
    {
        Debug.Log(playerSens);
        playerSens = (float) (System.Math.Round(sens, 2));
        txt.text = playerSens.ToString();
    }

    // getter method for camera controller to set sens
    public static float GetSensitivity()
    {
        return playerSens;
    }
}
