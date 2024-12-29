using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningObstacle : MonoBehaviour
{
    public float rotationSpeed = 100f;

    public float knockBackForce = 5f;

    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    public RotationAxis rotationAxis = RotationAxis.Y;

    void Update()
    {
        // Set rotation direction based on the selected axis
        Vector3 rotationVector = Vector3.zero;

        switch (rotationAxis)
        {
            case RotationAxis.X:
                rotationVector = Vector3.right;
                break;
            case RotationAxis.Y:
                rotationVector = Vector3.up;
                break;
            case RotationAxis.Z:
                rotationVector = Vector3.forward;
                break;
        }

         // Rotate the obstacle
        transform.Rotate(rotationVector, rotationSpeed * Time.deltaTime);
    }

    // Apply knockback to player on collision
    void OnCollisionEnter(Collision collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            Vector3 knockbackDirection = (collision.transform.position - transform.position).normalized;
            player.Knockback(knockbackDirection * knockBackForce);
        }
    }
}