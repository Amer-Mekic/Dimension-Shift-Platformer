using UnityEngine;
using System.Collections;

public class AN_DoorScript : MonoBehaviour
{
    [Tooltip("If true, this door is remotely controlled")]
    public bool Remote = true;

    [Space]
    [Tooltip("Door can be opened")]
    public bool CanOpen = true;

    [Space]
    [Tooltip("Current status of the door")]
    public bool isOpened = false;

    private bool isAnimating = false;
    private Vector3 closedRotation;
    private Vector3 openRotation;
    [Tooltip("If true, all Rigidbody position and rotation constraints will be applied automatically")]
    public bool FreezeAllConstraints = true;
    void Start()
    {
        closedRotation = new Vector3(-90, 90, 0); // Initial closed rotation
        openRotation = new Vector3(-90, 90, 90);  // Final open rotation (90 degrees)
        transform.rotation = Quaternion.Euler(closedRotation); // Start with the door closed
        
        // Freeze all constraints at the beginning (rotation & position), 
        // so the door cannot be just pushed open by force.
        if (FreezeAllConstraints)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

    public void OpenDoor() // Open the door
    {
        if (CanOpen && !isOpened) // Only open the door if it’s not already open
        {
            isOpened = true;  // Set the door as opened
            StartCoroutine(RotateDoor(openRotation));  // Start the door opening animation
        }
    }

    private IEnumerator RotateDoor(Vector3 targetRotation)
    {
        isAnimating = true;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(targetRotation);
        float elapsedTime = 0f;
        float duration = 3f;

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation; // Ensure the door reaches the target rotation
        isAnimating = false;
    }
}