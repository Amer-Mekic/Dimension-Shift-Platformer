using UnityEngine;
using UnityEngine.SceneManagement;

public class Spikes : MonoBehaviour
{
    public AudioClip deathSound;
    private AudioSource audioSource;
    void Start() {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(deathSound);
            Destroy(other.gameObject, 0.5f); // Removes the player from the scene / game over
            SceneManager.LoadScene("GameOver");
        }
    }
}
