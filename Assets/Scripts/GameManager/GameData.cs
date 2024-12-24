using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Saves the state of the game
[System.Serializable]
public class GameData
{
    public int level;
    public bool isEasy;
    public bool isHard;
    public float sensitivity;
    public int secretTreasure;

    public GameData (GameManager game)
    {
        level = game.currentLvl;
        isEasy = game.isEasy;
        isHard = game.isHard;
        sensitivity = game.sensitivity;
        secretTreasure = game.secretTreasure;
    }
}
