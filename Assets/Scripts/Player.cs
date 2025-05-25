using UnityEngine;

public class Player : MonoBehaviour
{
    // Ground Movement
    private Rigidbody rb;
    public float MoveSpeed = 5f;

    // Jumping
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float ascendMultiplier = 2f;
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    public float groundCheckDelay = 0.3f;
    private float playerHeight;
    private float raycastDistance;

    // Constant gravity
    public float extraGravity = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        playerHeight = GetComponent<CapsuleCollider>().height * transform.localScale.y;
        raycastDistance = (playerHeight / 2) + 0.3f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (groundCheckTimer <= 0f)
        {
            isGrounded = CheckIfGrounded();
        }
        else
        {
            groundCheckTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
        ApplyJumpPhysics();
        ApplyConstantGravity();
    }

    void MovePlayer()
    {
        float moveDirection = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            moveDirection = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection = 1f;
        }

        Vector3 movement = transform.right * moveDirection * MoveSpeed;
        Vector3 velocity = rb.velocity;
        velocity.x = movement.x;
        velocity.z = 0; // Locked Z axis
        rb.velocity = velocity;

        if (isGrounded && moveDirection == 0)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void Jump()
    {
        isGrounded = false;
        groundCheckTimer = groundCheckDelay;

        Vector3 jumpVelocity = rb.velocity;
        jumpVelocity.y = jumpForce;
        rb.velocity = jumpVelocity;
    }

    void ApplyJumpPhysics()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (ascendMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void ApplyConstantGravity()
    {
        rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
    }

    bool CheckIfGrounded()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        Debug.DrawRay(rayOrigin, Vector3.down * raycastDistance, Color.red);

        if (Physics.SphereCast(rayOrigin, 0.3f, Vector3.down, out RaycastHit hit, raycastDistance, groundLayer))
        {
            return Vector3.Angle(hit.normal, Vector3.up) < 45f;
        }

        return false;
    }
}
