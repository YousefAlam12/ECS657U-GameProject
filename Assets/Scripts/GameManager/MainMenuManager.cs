using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Loads main menu screen
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }
}
