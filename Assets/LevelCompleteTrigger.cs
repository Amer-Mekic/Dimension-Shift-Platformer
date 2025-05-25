using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteTrigger : MonoBehaviour
{
    [Tooltip("Name of the scene to load when level is completed.")]
    public string levelCompleteSceneName = "LevelComplete"; // Change this to match your scene name

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[LevelCompleteTrigger] Player reached the finish line.");
            SceneManager.LoadScene(levelCompleteSceneName);
        }
    }
}
