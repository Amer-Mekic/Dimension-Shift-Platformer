using UnityEngine;

public class CratePushTrigger : MonoBehaviour
{
    public Crate crate; // Reference to the Crate script
    private bool isPlayerNearby = false;
    private Transform player; // Reference to the player

    private void Start()
    {
        if (crate == null)
        {
            crate = GetComponentInParent<Crate>(); // Automatically link to the Crate script if not set in inspector
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[CratePushTrigger] Player entered the trigger zone.");
            isPlayerNearby = true;
            player = other.transform; // Store the player's transform for direction calculation
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[CratePushTrigger] Player exited the trigger zone.");
            isPlayerNearby = false;
            player = null; // Reset player reference when leaving
        }
    }

    private void Update()
    {
        if (isPlayerNearby && crate != null && player != null)
        {
            // Calculate the direction to move the crate based on player's position relative to the crate
            Vector3 direction = player.position - crate.transform.position;

            // Move the crate depending on whether the player is on the left or right of the crate
            if (direction.x > 0) // Player is to the right of the crate
            {
                crate.MoveCrate(Vector3.left); // Push the crate to the left
            }
            else if (direction.x < 0) // Player is to the left of the crate
            {
                crate.MoveCrate(Vector3.right); // Push the crate to the right
            }
        }
    }
}
