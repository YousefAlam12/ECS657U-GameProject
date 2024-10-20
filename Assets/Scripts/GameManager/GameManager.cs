using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public HealthManager healthManager;
    public GameObject mainCamera;
    public GameObject gameOverPanel;
    public PlayerInventory playerInventory;
    public Button restartButton;
    public Button mainMenuButton;
    public TextMeshProUGUI gameOverText; 

    void Start() 
    {
        gameOverPanel = GameObject.Find("GameOverPanel");
        healthManager = FindAnyObjectByType<HealthManager>();
        playerInventory = FindAnyObjectByType<PlayerInventory>();

        // Hide Game Over panel initially
        gameOverPanel.SetActive(false);
        gameOverText.gameObject.SetActive(false);

        healthManager = FindAnyObjectByType<HealthManager>();
        mainCamera = Camera.main.gameObject;
    }

    void Update() 
    {
        if (healthManager.currentHealth <= 0) {
            GameOver();
        }

        // Transisitons to lvl2 once the treasure is obtained from lvl1
        if (playerInventory.NumberOfTreasure == 1 && SceneManager.GetActiveScene().name != "Level2") {
            SceneManager.LoadScene("Level2");
        }

        // Prototype finish screen 
        if (playerInventory.NumberOfTreasure == 2)
        {
            SceneManager.LoadScene("MainMenuScreen");
        }
    }

    void GameOver() 
    {
        // Display Game Over panel
        gameOverPanel.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        Time.timeScale = 0f; // Pause the game

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mainCamera.GetComponent<CameraController>().enabled = false;
    }

    void Awake()
    {
        Time.timeScale = 1f; // Resume game on scene load
    }

    public void RestartGame() 
    {
        // Restart game from level 1
        SceneManager.LoadScene("Level1");
    }

    public void MainMenu() 
    {
        // Restart game from level 1
        SceneManager.LoadScene("MainMenuScreen");
    }
}