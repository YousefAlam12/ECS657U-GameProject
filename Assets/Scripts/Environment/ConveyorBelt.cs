using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public Vector3 conveyorDirection = new Vector3(1, 0, 0); // Direction of the conveyor movement
    public float conveyorSpeed = 2f; // Speed of the conveyor

    private string playerTag = "Player"; // Tag to identify the player
    private Dictionary<Rigidbody, Coroutine> affectedObjects = new Dictionary<Rigidbody, Coroutine>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // Set the player's force in the direction of the conveyor's movement
                player.SetConveyorForce(conveyorDirection.normalized * conveyorSpeed);
            }
        }
    }

    // stop following the conveyor force on exit
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.SetConveyorForce(Vector3.zero);
            }
        }
    }


    private IEnumerator MoveWithConveyor(Rigidbody rb)
    {
        while (true)
        {
            // Apply force in the conveyor's direction
            rb.AddForce(conveyorDirection.normalized * conveyorSpeed, ForceMode.Force);

            // Wait for the next FixedUpdate
            yield return new WaitForFixedUpdate();
        }
    }
}
