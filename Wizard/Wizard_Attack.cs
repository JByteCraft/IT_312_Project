using UnityEngine;

// Handles the wizard's attack mechanics, including cooldown and fireball firing
public class Wizard_Attack : MonoBehaviour
{
    // Time required between attacks
    [SerializeField] private float attackCooldown;

    // The point where the fireball will be spawned
    [SerializeField] private Transform firePoint;

    // Pool of fireball GameObjects to be reused (object pooling for performance)
    [SerializeField] private GameObject[] fireballs;

    // Animator to trigger attack animation
    private Animator anim;

    // Reference to the Wizard_Movements script to check if the player can attack
    private Wizard_Movements playerMovement;

    // Tracks time since last attack to enforce cooldown
    private float cooldownTimer = Mathf.Infinity;

    // Called when the script is initialized
    void Awake()
    {
        // Get the Animator component on the same GameObject
        anim = GetComponent<Animator>();

        // Get the Wizard_Movements script on the same GameObject
        playerMovement = GetComponent<Wizard_Movements>();
    }

    // Called once per frame
    void Update()
    {
        // Check for spacebar input, cooldown elapsed, and if the player can attack
        if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer > attackCooldown && playerMovement.canAttack())
        {
            Attack(); // Perform attack
        }

        // Increment the cooldown timer over time
        cooldownTimer += Time.deltaTime;
    }

    // Handles attack logic
    private void Attack()
    {
        // Trigger the attack animation
        anim.SetTrigger("attack");

        // Reset cooldown timer after attack
        cooldownTimer = 0;

        // Position the selected fireball at the firePoint and activate it
        fireballs[FindFireball()].transform.position = firePoint.position;

        // Set the direction of the fireball based on the player's facing direction
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    // Finds the next available fireball from the pool that is inactive (not in use)
    private int FindFireball()
    {
        // Loop through fireball pool
        for (int i = 0; i < fireballs.Length; i++)
        {
            // If the fireball is not active, return its index
            if (!fireballs[i].activeInHierarchy)
            {
                return i;
            }
        }

        // If all fireballs are active, return the first one (as fallback)
        return 0;
    }
}
