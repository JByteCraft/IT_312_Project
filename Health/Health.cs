using System.Collections;
using UnityEngine;

// This script manages a character's health system, including taking damage, healing, 
// triggering death or hurt animations, and handling invulnerability frames (iFrames).
public class Health : MonoBehaviour
{
    [Header("Health")]

    // The maximum health value the character starts with (set via the Unity Inspector).
    [SerializeField] private float startingHealth;

    // The character's current health. Publicly readable but only modifiable within this script.
    public float currentHealth { get; private set; }

    // Reference to the Animator component used for triggering animations (e.g., hurt, dead).
    private Animator anim;

    // Tracks if the character has already died to prevent triggering death logic multiple times.
    private bool dead;

    [Header("iFrames")]

    // Duration (in seconds) that the character is invulnerable after taking damage.
    [SerializeField] private float iFramesDuration;

    // Number of flashes the sprite will make during the invulnerability period.
    [SerializeField] private int numberOfFlashes;

    // Reference to the SpriteRenderer used to visually flash the character on damage.
    private SpriteRenderer spriteRend;

    // This method is automatically called when the GameObject is initialized (before Start).
    private void Awake()
    {
        // Initialize current health to the maximum starting value.
        currentHealth = startingHealth;

        // Get reference to Animator component attached to the same GameObject.
        anim = GetComponent<Animator>();

        // Get reference to SpriteRenderer to later flash the character when damaged.
        spriteRend = GetComponent<SpriteRenderer>();
    }

    // Call this method to apply damage to the character.
    public void TakeDamage(float _damage)
    {
        // Subtract damage from current health but clamp the result between 0 and startingHealth.
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        // If the character is still alive after taking damage:
        if (currentHealth > 0)
        {
            // Trigger the "hurt" animation.
            anim.SetTrigger("hurt");

            // Start invulnerability period to prevent taking more damage immediately.
            StartCoroutine(Invulnerability());
        }
        else
        {
            // If the character has died and hasn't already been marked dead:
            if (!dead)
            {
                // Trigger the "dead" animation.
                anim.SetTrigger("dead");

                if (GetComponent<Wizard_Movements>() != null)
                {
                    // Disable the movement script so the character can't move anymore.
                    GetComponent<Wizard_Movements>().enabled = false;
                }

                if (GetComponentInParent<Patrol>() != null)
                {
                    GetComponentInParent<Patrol>().enabled = false;

                }

                if (GetComponent<Swordsman>() != null)
                {
                    GetComponent<Swordsman>().enabled = false;
                }

                // Set the dead flag to true to prevent this block from running again.
                dead = true;
            }
        }
    }

    // Call this method to heal the character by a specified value.
    public void AddHealth(float _value)
    {
        // Add health, making sure it doesn't go above the starting health.
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    // Coroutine that handles temporary invulnerability after taking damage.
    private IEnumerator Invulnerability()
    {
        // Disable collision between layers 10 and 11 (example: player and enemy attacks).
        Physics2D.IgnoreLayerCollision(10, 11, true);

        // Flash the sprite multiple times to show that the character is invulnerable.
        for (int i = 0; i < numberOfFlashes; i++)
        {
            // Change color to red with transparency to simulate a damage flash.
            spriteRend.color = new Color(1, 0, 0, 0.5f);

            // Wait for a short time before changing back.
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));

            // Reset the sprite color to normal.
            spriteRend.color = Color.white;

            // Wait again before next flash.
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        // Re-enable collisions between layers 10 and 11 after invulnerability ends.
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
}
