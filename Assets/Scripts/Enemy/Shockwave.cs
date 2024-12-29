using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public float expansionSpeed = 5f; // Speed at which the shockwave expands
    public float maxScale = 20f;      // Maximum scale for the shockwave

    private Vector3 initialScale;

    void Start()
    {
        // Store the initial scale of the shockwave
        initialScale = transform.localScale;
    }

    void Update()
    {
        // Expand the shockwave over time
        transform.localScale += new Vector3(expansionSpeed * Time.deltaTime, 0, expansionSpeed * Time.deltaTime);

        // Lock the Y-axis scale to its initial value
        transform.localScale = new Vector3(transform.localScale.x, initialScale.y, transform.localScale.z);

        // Destroy the shockwave when it exceeds the max scale
        if (transform.localScale.x >= maxScale || transform.localScale.z >= maxScale)
        {
            Destroy(gameObject);
        }
    }
}

