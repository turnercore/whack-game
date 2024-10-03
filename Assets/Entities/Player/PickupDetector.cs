using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PickupDetector : MonoBehaviour
{
    private void Awake()
    {
        // Ensure the CircleCollider2D is set as a trigger
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is tagged as "Pickup"
        if (other.CompareTag("Pickup"))
        {            
            // Ensure pickup is not null before calling the method
            if (other.TryGetComponent<Pickup>(out var pickup))
            {
                // Call the Pickup method, passing this gameObject as the player
                pickup.PickupItem(GameManager.Instance.GetPlayerObject());
            }
        }
    }
}
