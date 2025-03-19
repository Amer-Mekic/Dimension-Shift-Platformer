using UnityEngine;

// Base class that coin and star scripts extend(inherit from)
// Used to refactor code after figuring that the logic for coin and star is essentially the same.
public class Collectible : MonoBehaviour
{
    public AudioClip collectSound; // "public" to create a field in Unity editor to use custom sound.
    protected AudioSource audioSource; // Only base & child classes should access this field, hence "protected"
    
    void Start() {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Open/Closed principle - open for extension, closed for modification.
    protected virtual void Collect()
    {
        audioSource.PlayOneShot(collectSound); // Play sound
        GetComponent<MeshRenderer>().enabled = false; // Hide object
        GetComponent<Collider>().enabled = false; // Disable collider
        Destroy(gameObject, 0.5f); // Delay destruction to let sound play
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect(); // Call collect method when player collides with object.
        }
    }
}
