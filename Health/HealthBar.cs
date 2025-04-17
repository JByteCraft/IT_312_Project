using UnityEngine;
using UnityEngine.UI;

// This script handles the visual representation of the player's health using UI Images
public class HealthBar : MonoBehaviour
{
    // Reference to the player's Health script to read the current health value
    [SerializeField] private Health playerHealth;

    // Reference to the UI image that represents the total health (usually the background or the full bar)
    [SerializeField] private Image totalHealthBar;

    // Reference to the UI image that shows the current health (usually the front bar that decreases)
    [SerializeField] private Image currentHealthBar;

    // Called before the first frame update
    void Start()
    {
        // Set the total health bar's initial value based on the player's starting health
        // Assumes max health is 10, so the fill amount becomes 1.0 if health is full
        totalHealthBar.fillAmount = playerHealth.currentHealth / 10;
    }

    // Called once per frame
    void Update()
    {
        // Update the current health bar's fill amount based on the player's current health
        // This makes the health bar smoothly shrink as the player takes damage
        currentHealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
}
