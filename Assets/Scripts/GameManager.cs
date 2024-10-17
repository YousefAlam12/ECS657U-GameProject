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
    public Button restartButton;
    public TextMeshProUGUI gameOverText; 

    void Start() 
    {
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
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}