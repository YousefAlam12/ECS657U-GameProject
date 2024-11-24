using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollision : MonoBehaviour
{
    private string playerTag = "Player";
    [SerializeField] Transform platform;

    private bool playerEnters = false;
    private bool playerExits = false;
    private Collider obj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
        //     other.gameObject.transform.parent = platform;
            playerEnters = true;
            obj = other;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
        //     other.gameObject.transform.parent = null;
            playerExits = true;
            obj = other;
        }

    }

    // When player enters the platform make them move with it
    void PlayerEnters() {
        // obj.gameObject.transform.parent = platform;
        obj.gameObject.transform.SetParent(platform, true);

        playerEnters = false;
    }

    // When player leaves platform make its transform seperate from the platform
    void PlayerExits() {
        // obj.gameObject.transform.parent = null;
        obj.gameObject.transform.SetParent(null);
        playerExits = false;
    }


    // void LateUpdate() {
    void FixedUpdate() {
        if (playerEnters) {
            PlayerEnters();
        }

        if (playerExits) {
            PlayerExits();
        }
    }
}