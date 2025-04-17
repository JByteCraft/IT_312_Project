using UnityEngine;

// Handles the movement, collision, and behavior of a single fireball projectile
public class Projectile : MonoBehaviour
{
    // Speed at which the projectile moves
    [SerializeField] private float speed;

    // Direction the projectile is moving in (1 for right, -1 for left)
    private float direction;

    // Whether the projectile has already hit something
    private bool hit;

    // Timer to track how long the projectile has been active
    private float lifetime;

    // Reference to Animator component for playing explosion animation
    private Animator anim;

    // Reference to the BoxCollider2D for hit detection
    private BoxCollider2D boxCollider;

    // Called when the script is first initialized
    void Awake()
    {
        // Get the Animator attached to this GameObject
        anim = GetComponent<Animator>();

        // Get the BoxCollider2D attached to this GameObject
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Called every frame
    void Update()
    {
        // If the projectile has already hit something, stop updating its movement
        if (hit)
        {
            return;
        }

        // Move the projectile horizontally based on direction and speed
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        // Increase the lifetime counter
        lifetime += Time.deltaTime;

        // If the projectile has been alive for more than 3 seconds, deactivate it
        if (lifetime > 3)
        {
            gameObject.SetActive(false);
        }
    }

    // Called when the projectile collides with another collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Mark the projectile as hit so it stops moving
        hit = true;

        // Disable the collider so it doesn't keep detecting collisions
        boxCollider.enabled = false;

        // Trigger the explosion animation
        anim.SetTrigger("explode");
    }

    // Sets the direction of the projectile when fired
    public void SetDirection(float _direction)
    {
        // Reset lifetime to start the countdown again
        lifetime = 0;

        // Set movement direction
        direction = _direction;

        // Reactivate the projectile
        gameObject.SetActive(true);
        hit = false;

        // Re-enable the collider in case it was disabled after hitting
        boxCollider.enabled = true;

        // Flip the projectile if it's facing the wrong way
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }

        // Apply the flipped scale
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    // Deactivates the projectile (usually called at the end of explosion animation)
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
