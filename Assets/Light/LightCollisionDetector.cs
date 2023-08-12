using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCollisionDetector : MonoBehaviour
{
    // Declare a public float variable to store the range of the light source
    public float lightRange = 10f;

    void Update()
    {
        // Detect all the colliders within the range of the light source
        Collider[] colliders = Physics.OverlapSphere(transform.position, lightRange);
        foreach (Collider collider in colliders)
        {
            // Check if the detected collider belongs to an object tagged as "Player"
            if (collider.gameObject.CompareTag("Player"))
            {
                // Call the PlayerDetected method
                PlayerDetected();
            }
        }
    }

    void PlayerDetected()
    {
        // Print a message to the console
        Debug.Log("Player entered the light range!");
    }

    // Draw a yellow wire sphere in the Unity Editor to represent the range of the light source
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lightRange);
    }
}