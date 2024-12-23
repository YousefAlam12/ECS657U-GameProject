using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // public static float sensitivity;
    public static bool easyMode = false;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

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

    public void LoadGame()
    {
        GameData data = SaveSystem.LoadGame();
        
        easyMode = data.isEasy;
        OptionsManager.playerSens = data.sensitivity;
        string lvl = "Level" + data.level;
        PlayerInventory.SecretTreasure = data.secretTreasure;
        SceneManager.LoadScene(lvl);
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

