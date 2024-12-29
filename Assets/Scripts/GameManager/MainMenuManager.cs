using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // public static float sensitivity;
    public static bool easyMode = false;
    public static bool hardMode = false;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
        PlayerInventory.SecretTreasure = 0;
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
        hardMode = false;
    }

    // sets difficulty to standard when button is clicked
    public void setStandard()
    {
        easyMode = false;
        hardMode = false;
    }

    // sets difficulty to hard when button is clicked
    public void setHard()
    {
        easyMode = false;
        hardMode = true;
    }

    // returns ans to if game is on easy mode
    public static bool isEasy()
    {
        return easyMode;
    }

    // returns ans to if game is on hard mode
    public static bool isHard()
    {
        return hardMode;
    }

    // Uses the gamedata that was saved to load the game from the last lvl the player left off
    public void LoadGame()
    {
        GameData data = SaveSystem.LoadGame();
        
        easyMode = data.isEasy;
        hardMode = data.isHard;
        OptionsManager.playerSens = data.sensitivity;
        string lvl = "Level" + data.level;
        PlayerInventory.SecretTreasure = data.secretTreasure;
        SceneManager.LoadScene(lvl);
    }
}

