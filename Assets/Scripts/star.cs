using UnityEngine;

public class Star : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Star Collected!");
            Destroy(gameObject);
        }
    }
}