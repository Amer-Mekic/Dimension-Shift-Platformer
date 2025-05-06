using UnityEngine;

public class ObjectBounce : MonoBehaviour
{
    public float bounceSpeed = 8;
    public float bounceAmplitude = 0.05f;
    public float rotationSpeed = 90;

    private float startHeight;
    private float timeOffset;

    void Start()
    {
        startHeight = (float)(transform.localPosition.y+0.3);
        timeOffset = Random.value * Mathf.PI * 2;
    }

    void Update()
    {
        //animate
        float finalheight = startHeight + Mathf.Sin(Time.time * bounceSpeed + timeOffset) * bounceAmplitude;
        var position = transform.localPosition;
        position.y = finalheight;
        transform.localPosition = position;

        //spin
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotation.y += rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }
}
