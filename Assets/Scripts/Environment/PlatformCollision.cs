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
    private GameObject temporaryParent; // Temporary GameObject to parent the player

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag) && temporaryParent == null)
        {
            playerEnters = true;
            obj = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            playerExits = true;
            obj = other;
        }
    }

    // When player enters the platform, make them a child of a temporary parent to move with the platform - prevents player distortion
    void PlayerEnters()
    {
        if (obj != null && temporaryParent == null)
        {
            // Create a new temporary parent GameObject
            temporaryParent = new GameObject("TemporaryParent");

            // Reset aspects of the temporary parent to prevent distortions
            temporaryParent.transform.localScale = Vector3.one;
            temporaryParent.transform.position = platform.position;
            temporaryParent.transform.rotation = platform.rotation;

            // Move the player into the temporary parent
            temporaryParent.transform.SetParent(platform);
            obj.gameObject.transform.SetParent(temporaryParent.transform, true);

            playerEnters = false;
        }
    }

    // When player leaves the platform, remove the temporary parent
    void PlayerExits()
    {
        if (obj != null && temporaryParent != null)
        {
            // Detach the player from the temporary parent
            obj.gameObject.transform.SetParent(null);

            // Destroy the temporary parent GameObject
            Destroy(temporaryParent);
            temporaryParent = null;

            playerExits = false;
        }
    }

    void FixedUpdate()
    {
        if (playerEnters)
        {
            PlayerEnters();
        }

        if (playerExits)
        {
            PlayerExits();
        }
    }
}
