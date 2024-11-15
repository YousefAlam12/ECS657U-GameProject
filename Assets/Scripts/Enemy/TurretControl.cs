using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform player;
    public float aggroRange = 10.0f;
    public Transform turretHead;
    public float alignmentThreshold = 0.1f; // Threshold to avoid jitter

    private Vector3 lastDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= aggroRange) // turret will look at player if within range
        {
            Vector3 directionToTarget = (player.position - turretHead.position).normalized;

            // Check if the direction has changed significantly
            if (Vector3.Distance(directionToTarget, lastDirection) > alignmentThreshold)
            {
                turretHead.LookAt(player.position);
                lastDirection = directionToTarget;
            }
        }
    }
}
