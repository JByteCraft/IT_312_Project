using UnityEngine;

// This script controls an enemy that moves side-to-side between two points
public class Saw : MonoBehaviour
{
    [SerializeField] private float movementDistance; // How far the enemy moves left and right from its starting point
    [SerializeField] private float speed; // Movement speed of the enemy
    [SerializeField] private float damage; // How much damage it deals to the player on contact

    private bool movingLeft; // Direction flag for movement
    private float leftEdge; // Left limit of movement
    private float rightEdge; // Right limit of movement

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Set the bounds the enemy can move between, based on its starting position
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    // Update is called once per frame
    private void Update()
    {
        if (movingLeft)
        {
            // If not yet at the left edge, keep moving left
            if (transform.position.x > leftEdge)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                // Switch direction when the edge is reached
                movingLeft = false;
            }
        }
        else
        {
            // If not yet at the right edge, keep moving right
            if (transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                // Switch direction when the edge is reached
                movingLeft = true;
            }
        }
    }

    // Called when another collider enters this object's trigger collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Deal damage to the player when touched
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
