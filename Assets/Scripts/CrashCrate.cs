using UnityEngine;

public class Crate : MonoBehaviour
{
    [Header("Crate Components")]
    public BoxCollider boxCollider;
    public MeshRenderer wholeCrate;
    public GameObject fracturedCrate;
    public AudioSource crashAudioClip;

    [Header("Crate Settings")]
    public float pushDistance = 1f; // Move 1x (1 block)
    public float fallHeightThreshold = 3f; // Height required to break
    private Vector3 lastPosition;
    private bool isGrounded = false;
    private bool isFalling = false;
    private float highestPoint;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        CheckIfFalling();
        lastPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 direction = collision.contacts[0].point - transform.position;
            direction = direction.normalized;

            // Push only if the player collides from the left or right
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                Push(direction.x);
            }
        }
    }

    private void Push(float directionX)
    {
        if (rb == null) return; // Safety check

        Vector3 force = new Vector3(Mathf.Sign(directionX) * pushDistance, 0, 0);
        rb.AddForce(force, ForceMode.Impulse);

        // Force rechecking if the crate is now falling
        if (!Physics.Raycast(transform.position, Vector3.down, 0.6f))
        {
            isFalling = true;
            rb.useGravity = true;
            highestPoint = transform.position.y; // Set new highest point after push
        }
    }

    private void CheckIfFalling()
{
    bool wasGrounded = isGrounded;
    isGrounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.6f);

    if (!isGrounded && !isFalling)
    {
        isFalling = true;
        rb.useGravity = true;
        highestPoint = transform.position.y; // Store height BEFORE falling
        Debug.Log("Crate started falling! Highest Point: " + highestPoint);
    }

    if (isFalling && isGrounded)
    {
        isFalling = false;

        float fallDistance = highestPoint - transform.position.y; // Correct calculation
        Debug.Log("Crate landed! Fall distance: " + fallDistance);

        if (fallDistance >= fallHeightThreshold)
        {
            Debug.Log("Crate breaking, fall distance met threshold!");
            BreakCrate();
        }
    }

    // Apply horizontal drag only while falling
    if (isFalling)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x * 0.1f, rb.linearVelocity.y, rb.linearVelocity.z);
    }

    // Update highest point while crate is in the air
    if (isFalling && transform.position.y > highestPoint)
    {
        highestPoint = transform.position.y; // Update only if crate moves higher
    }
}



    private void BreakCrate()
    {
        wholeCrate.enabled = false;
        boxCollider.enabled = false;
        fracturedCrate.SetActive(true);
        crashAudioClip.Play();
        Destroy(gameObject, 2f); // Destroy after playing effect
    }

    [ContextMenu("Test Break")]
    public void TestBreak()
    {
        BreakCrate();
    }
}
