using UnityEngine;

public class CoinJump : MonoBehaviour
{
    public float amplitude = 0.5f;       // Visina skoka
    public float frequency = 2f;         // Brzina skoka
    public float rotationSpeed = 180f;   // Brzina rotacije

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Skakanje gore-dole
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);

        // Rotacija oko Z ose (kao disk koji se okreæe)
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
