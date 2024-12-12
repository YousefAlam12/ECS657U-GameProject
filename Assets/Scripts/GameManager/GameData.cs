using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Saves the state of the game
[System.Serializable]
public class GameData
{
    public int level;
    public bool isEasy;
    public float sensitivity;

    public GameData (GameManager game)
    {
        level = game.currentLvl;
        isEasy = game.isEasy;
        sensitivity = game.sensitivity;
    }
}
