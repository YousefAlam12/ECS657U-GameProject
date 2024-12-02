using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static float sensitivity;
    public static bool easyMode = false;

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OpenControls()
    {
        SceneManager.LoadScene("ControlsScreen");
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void CloseControls()
    {
        SceneManager.LoadScene("MainMenuScreen");
    }

    // sets difficulty to easy when button is clicked
    public void setEasy()
    {
        easyMode = true;
    }

    // sets difficulty to standard when button is clicked
    public void setStandard()
    {
        easyMode = false;
    }

    public static bool isEasy()
    {
        return easyMode;
    }

    void Update()
    {
        ////////////////////////////////////////////
        // ONLY FOR PLAYTEST
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene("Yousef");
        }
        ////////////////////////////////////////////
    }
}

