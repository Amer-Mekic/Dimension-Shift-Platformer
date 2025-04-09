using UnityEngine;

public class AN_Button : MonoBehaviour
{
    public bool isLever = false;  // Whether this is a lever button or a regular button
    public AN_DoorScript DoorObject;  // Reference to the door object

    private Animator anim;
    public bool playerNear = false; // Track if player is near the button

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if player is near the button and presses the E key
        if (playerNear && Input.GetKeyDown(KeyCode.E) && DoorObject != null)
        {
            Debug.Log("E Button Pressed!"); // Check if button press is detected

            // Play button animation when E is pressed
            if (isLever)
            {
                anim.SetBool("LeverUp", !anim.GetBool("LeverUp"));
            }
            else
            {
                anim.SetTrigger("ButtonPress");
            }

            // Trigger the door opening if it's not already open
            if (!DoorObject.isOpened)
            {
                DoorObject.OpenDoor();  // Open the door
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            Debug.Log("Player Entered Button Trigger Area"); // Player near button
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            Debug.Log("Player Left Button Trigger Area"); // Player leaves button trigger
        }
    }

    public void OpenDoor()
    {
        if (DoorObject != null)
        {
            DoorObject.OpenDoor(); // Open the door
        }
    }
}