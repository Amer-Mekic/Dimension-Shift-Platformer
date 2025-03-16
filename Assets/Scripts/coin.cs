using UnityEngine;

public class Coin : MonoBehaviour
{
    public AudioClip coinSound;
    private AudioSource audioSource;
    void Start() {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    public void CollectCoin(){
        Destroy(gameObject, 0.5f); // delay collecting 0.5s to play sound
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(coinSound); // Play sound
            GetComponent<MeshRenderer>().enabled = false; // Hide coin
            GetComponent<Collider>().enabled = false; // Disable collider
            CollectCoin();
        }
    }
}