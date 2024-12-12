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
    public Vector3 respawnPoint;
    public GameObject player;

    // data to save the state of the game
    public int currentLvl;
    public bool isEasy;
    public float sensitivity;


    // flag for respawn
    private bool hasRespawned = false;

    void Start() 
    {
        gameOverPanel = GameObject.Find("GameOverPanel");
        healthManager = FindAnyObjectByType<HealthManager>();
        playerInventory = FindAnyObjectByType<PlayerInventory>();
        player = GameObject.Find("Player");

        // Hide Game Over panel initially
        gameOverPanel.SetActive(false);
        gameOverText.gameObject.SetActive(false);

        healthManager = FindAnyObjectByType<HealthManager>();
        mainCamera = Camera.main.gameObject;

        respawnPoint = player.transform.position;

        // Varibales to hold current state of game
        isEasy = MainMenuManager.isEasy();
        sensitivity = OptionsManager.GetSensitivity();

        // Save the game state upon the start of every level
        SaveSystem.SaveGame(this);
    }

    void Update() 
    {
        if (healthManager.currentHealth <= 0) {
            GameOver();
        }

        if (playerInventory.NumberOfTreasure == 1 && !hasRespawned)
        {
            RespawnPlayer();
            hasRespawned = true;
            
        }


        // Transisitons to lvl2 once the treasure is obtained from lvl1
        // if (playerInventory.NumberOfTreasure == 2 && SceneManager.GetActiveScene().name != "Level2") {
        if (playerInventory.NumberOfTreasure == playerInventory.totalTreasure && currentLvl == 1) {
            SceneManager.LoadScene("Level2");
        }

        // Prototype finish screen 
        if (playerInventory.NumberOfTreasure == playerInventory.totalTreasure && currentLvl == 2)
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

        // when game is standard mode ensure that player is not able to load back to the level they died on and start again
        if (!isEasy)
        {
            currentLvl = 1;
            SaveSystem.SaveGame(this);
        }
    }

    void Awake()
    {
        Time.timeScale = 1f; // Resume game on scene load
    }

    public void RestartGame() 
    {
        // respawn on current level when easy mode
        if(MainMenuManager.isEasy())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        // Restart game from level 1
        SceneManager.LoadScene("Level1");
    }

    public void MainMenu() 
    {
        // Restart game from level 1
        SceneManager.LoadScene("MainMenuScreen");
    }

    void RespawnPlayer()
    {
        if (player != null && respawnPoint != null)
        {
            player.transform.position = respawnPoint; // Move player to respawn point

            // Restore health on respaw when easy
            if (MainMenuManager.isEasy())
            {
                healthManager.healthBar.setHealth(healthManager.maxHealth);
            }
        }
    }

    // pauses the game
    public void Pause(bool isPaused)
    {
        if (isPaused)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            mainCamera.GetComponent<CameraController>().enabled = false;
        }
        else
        {
            gameOverPanel.SetActive(false);
            Time.timeScale = 1f;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            mainCamera.GetComponent<CameraController>().enabled = true;
        }
    }
}