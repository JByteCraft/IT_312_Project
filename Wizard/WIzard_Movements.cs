using UnityEngine;

// Controls the wizard character's movement and animations
public class Wizard_Movements : MonoBehaviour
{
    // Reference to the Rigidbody2D component for physics-based movement
    public Rigidbody2D body;

    // Movement speed and jump power, set via the inspector
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    // Animator to control animations
    private Animator anim;

    // BoxCollider2D used for collision detection
    private BoxCollider2D boxCollider;

    // Layer masks used to identify what is considered ground or a wall
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    // Cooldown timer to delay between wall jumps
    private float wallJumpCooldown;

    // Stores the horizontal input from the player
    private float horizontalInput;

    // Called when the script instance is being loaded
    void Awake()
    {
        // Gets the Rigidbody2D component attached to the GameObject
        body.GetComponent<Rigidbody2D>();

        // Gets the Animator component attached to the GameObject
        anim = GetComponent<Animator>();

        // Gets the BoxCollider2D component attached to the GameObject
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Called every frame to handle input and animation updates
    void Update()
    {
        // Get horizontal movement input (-1 to 1)
        horizontalInput = Input.GetAxis("Horizontal");

        // Flip the character sprite based on movement direction
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-2.5f, 2.5f, 2.5f);
        }

        // Update animation states
        anim.SetBool("run", horizontalInput != 0); // running animation
        anim.SetBool("grounded", isGrounded());    // grounded animation

        // Only allow movement if wall jump cooldown is low
        if (wallJumpCooldown < 0.2f)
        {
            // Move the character left or right
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            // If the character is touching a wall and is grounded, freeze them (e.g., for wall slide)
            if (onWall() && isGrounded())
            {
                body.gravityScale = 0;
                body.linearVelocity = Vector2.zero;
            }
            else
            {
                // Restore normal gravity when not on a wall
                body.gravityScale = 7;
            }

            // Jump if the up arrow is pressed
            if (Input.GetKey(KeyCode.UpArrow))
            {
                Jump();
            }
        }
        else
        {
            // Increment wall jump cooldown timer
            wallJumpCooldown += Time.deltaTime;
        }
    }

    // Handles jumping and wall jumping logic
    private void Jump()
    {
        // Regular jump when grounded
        if (isGrounded())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        // Wall jump when not grounded but touching a wall
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                // Neutral wall jump (no horizontal input)
                body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);

                // Flip the character's direction
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                // Wall jump with horizontal input
                body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            }

            // Reset the wall jump cooldown
            wallJumpCooldown = 0;
        }
    }

    // Checks if the character is currently on the ground using a box cast
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,      // Origin
            boxCollider.bounds.size,        // Size
            0,
            Vector2.down,                   // Direction
            0.1f,                           // Distance
            groundLayer);                   // What counts as ground

        return raycastHit.collider != null;
    }

    // Checks if the character is currently touching a wall using a box cast
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            new Vector2(transform.localScale.x, 0), // Direction depends on facing side
            0.1f,
            wallLayer);                      // What counts as wall

        return raycastHit.collider != null;
    }

    // Used to determine whether the character is in a position to attack
    public bool canAttack()
    {
        // Character can attack only when idle, grounded, and not touching a wall
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
