using UnityEngine;

// This script is attached to door objects that let the player transition between rooms.
// When the player enters the door's trigger area, the camera shifts to the next or previous room.
public class Door : MonoBehaviour
{
    // Reference to the room the door leads *from*
    [SerializeField] private Transform prevRoom;

    // Reference to the room the door leads *to*
    [SerializeField] private Transform nextRoom;

    // Reference to the Camera_Controller script that handles camera transitions
    [SerializeField] private Camera_Controller cam;

    // Awake is called when the script instance is being loaded (before Start)
    private void Awake()
    {
        // Get the Camera_Controller from the main camera in the scene
        cam = Camera.main.GetComponent<Camera_Controller>();
    }

    // This method is triggered when another collider enters this object's 2D trigger collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering the trigger is the player
        if (collision.tag == "Player")
        {
            // If the player is entering from the left side of the door,
            // transition the camera to the next room
            if (collision.transform.position.x < transform.position.x)
            {
                cam.MoveToNewRoom(nextRoom);
                nextRoom.GetComponent<Room>().ActivateRoom(true);
                prevRoom.GetComponent<Room>().ActivateRoom(false);
            }
            // If the player is entering from the right side of the door,
            // transition the camera to the previous room
            else
            {
                cam.MoveToNewRoom(prevRoom);
                prevRoom.GetComponent<Room>().ActivateRoom(true);
                nextRoom.GetComponent<Room>().ActivateRoom(false);
            }
        }
    }
}
