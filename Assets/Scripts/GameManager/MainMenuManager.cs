using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static float sensitivity;

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

