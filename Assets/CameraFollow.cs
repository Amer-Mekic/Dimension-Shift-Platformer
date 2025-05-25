using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private Vector3 offset;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("CameraFollow: Player not assigned!");
            return;
        }

        // Calculate initial offset from the player
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Only follow X position
            Vector3 targetPos = new Vector3(player.position.x + offset.x, transform.position.y, transform.position.z);
            transform.position = targetPos;
        }
    }
}
