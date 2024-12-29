using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForce = 15f; // The force applied to the player when they bounce on the pad

    // apply the bounce method in PlayerController to apply upwards bouncing force
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.Bounce(bounceForce);
        }
    }
}