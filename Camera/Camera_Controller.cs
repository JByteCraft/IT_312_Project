using UnityEngine;

// This script controls two types of camera behavior:
// 1. Smooth camera movement between rooms.
// 2. A (commented-out) follow-camera that tracks the player.
public class Camera_Controller : MonoBehaviour
{
    // ========== ROOM CAMERA VARIABLES ==========

    // Speed at which the camera moves when transitioning to a new room
    [SerializeField] private float speed;

    // Target x-position the camera should move to
    private float currentPosX;

    // Stores current movement velocity (used by SmoothDamp for smooth transitions)
    private Vector3 velocity = Vector3.zero;

    // ========== FOLLOW PLAYER CAMERA VARIABLES ==========

    // Reference to the player’s Transform (position, rotation, scale)
    [SerializeField] private Transform player;

    // Distance ahead of the player for the camera to look (creates a leading effect)
    [SerializeField] private float aheadDistance;

    // Speed at which the camera catches up when following the player
    [SerializeField] private float cameraSpeed;

    // Variable that stores the "look-ahead" value based on player movement
    private float lookAhead;

    // Called once at the start of the game
    void Start()
    {
        // Currently empty – you can add initialization logic here if needed
    }

    // Called once per frame
    void Update()
    {
        // ========== ROOM CAMERA MOVEMENT ==========

        // Smoothly transitions the camera’s X position to the target room's X position
        transform.position = Vector3.SmoothDamp(
            transform.position,
            new Vector3(currentPosX, transform.position.y, transform.position.z),
            ref velocity,
            speed
        );

        // ========== FOLLOW PLAYER CAMERA MOVEMENT ==========
        // This section is currently commented out
        // If enabled, the camera will follow the player and look slightly ahead in the direction they're facing

        // transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);

        // Smoothly update the lookAhead based on player's facing direction
        // lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }

    // This method is called externally (e.g., by a trigger or script) to shift the camera to a new room
    public void MoveToNewRoom(Transform _newRoom)
    {
        // Sets the new target X position to move the camera to
        // The value 3.04 might be an offset based on your level layout
        currentPosX = _newRoom.position.x + 3.04f;
    }
}
