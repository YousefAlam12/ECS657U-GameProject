using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameRespawn gameRespawn;
    [SerializeField] List<GameObject> checkPoints;

    void Start() {
        // player = FindAnyObjectByType<PlayerController>();
        player = GameObject.Find("Player");
    }


    // Updates the respawn point of the player when they collide with a checkpoint
    private void OnTriggerEnter(Collider other)
    {
        if (checkPoints.Contains(other.gameObject))
        {
            gameRespawn.UpdateRespawnPoint(player.transform.position);
            Debug.Log("Checkpoint triggered! New checkpoint set at: " + player.transform.position);
            Destroy(other.gameObject);
        }
    }
}