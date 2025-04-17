using UnityEngine;

// This script represents a collectible object that restores the player's health when picked up.
public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healthValue; // Amount of health restored when collected

    // Called when another collider enters this object's trigger collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered the trigger is the player
        if (collision.tag == "Player")
        {
            // Add health to the player using their Health component
            collision.GetComponent<Health>().AddHealth(healthValue);

            // Disable the collectible object after it's picked up
            gameObject.SetActive(false);
        }
    }
}
