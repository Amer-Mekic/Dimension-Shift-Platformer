using UnityEngine;

public class AN_Button : MonoBehaviour
{
    public bool isLever = false;  // Whether this is a lever button or a regular button
    public AN_DoorScript DoorObject;  // Reference to the door object

    private Animator anim;
    public bool playerNear = false; // Track if player is near the button

    [Header("Audio")]
    public AudioClip pressSound;          // Sound to play when button is pressed
    private AudioSource audioSource;      // Audio source component

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Add one if not present
        }
    }

    void Update()
    {
        // Check if player is near the button and presses the E key
        if (playerNear && Input.GetKeyDown(KeyCode.E) && DoorObject != null)
        {
            Debug.Log("E Button Pressed!"); // Check if button press is detected

            // Play button animation
            if (isLever)
            {
                anim.SetBool("LeverUp", !anim.GetBool("LeverUp"));
            }
            else
            {
                anim.SetTrigger("ButtonPress");
            }

            // Play sound
            if (pressSound != null)
            {
                audioSource.PlayOneShot(pressSound);
            }

            // Trigger the door opening if it's not already open
            if (!DoorObject.isOpened)
            {
                DoorObject.OpenDoor();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            Debug.Log("Player Entered Button Trigger Area");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            Debug.Log("Player Left Button Trigger Area");
        }
    }

    public void OpenDoor()
    {
        if (DoorObject != null)
        {
            DoorObject.OpenDoor();
        }
    }
}
