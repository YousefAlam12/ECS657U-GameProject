using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] GameObject pointA;
    [SerializeField] GameObject pointB;
    [SerializeField] float speed = 10f;
    [SerializeField] float delay = 1f;
    [SerializeField] GameObject platform;
    [SerializeField] float startDelay = 0f; // New variable to control start time
    [SerializeField] bool startFromPointA = true; // New variable to control initial point

    private Vector3 targetPosition;

    void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("Point A or Point B is not assigned in the inspector.");
            return;
        }

        // Set initial position based on startFromPointA
        platform.transform.position = startFromPointA ? pointA.transform.position : pointB.transform.position;
        targetPosition = startFromPointA ? pointB.transform.position : pointA.transform.position;

        // Wait for startDelay before starting the movement
        Invoke(nameof(StartMoving), startDelay);
    }

    void StartMoving()
    {
        StartCoroutine(MovePlatform());
    }

    IEnumerator MovePlatform()
    {
        while (true)
        {
            while ((targetPosition - platform.transform.position).sqrMagnitude > 0.01f)
            {
                platform.transform.position = Vector3.MoveTowards(platform.transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            targetPosition = targetPosition == pointA.transform.position ? pointB.transform.position : pointA.transform.position;
            yield return new WaitForSeconds(delay);
        }
    }
}