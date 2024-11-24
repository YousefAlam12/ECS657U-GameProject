using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfTreasure;
    public int NumberOfCoins;

    public void TreasureCollected()
    {
        NumberOfTreasure++;
    }
}
