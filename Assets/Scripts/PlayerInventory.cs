using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfTreasure { get; private set; }

    public void TreasureCollected()
    {
        NumberOfTreasure++;
    }
}