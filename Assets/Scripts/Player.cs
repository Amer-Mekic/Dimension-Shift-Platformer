using UnityEngine;

public class Player : MonoBehaviour
{

     // Ground Movement
    private Rigidbody rb;
    public float MoveSpeed = 5f;
    private float moveHorizontal;
    private float moveForward;

    // Jumping
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f; 
    public float ascendMultiplier = 2f; 
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    private float groundCheckDelay = 0.3f;
    private float playerHeight;
    private float raycastDistance;

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
        moveHorizontal = Input.GetAxisRaw("Horizontal");
     

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Updated ground check using SphereCast
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
    }

    void MovePlayer()
    {
        
        Vector3 movement = transform.right * moveHorizontal;
        Vector3 targetVelocity = movement * MoveSpeed;

        Vector3 velocity = rb.linearVelocity; // Using linearVelocity per new physics system
        velocity.x = targetVelocity.x;

        // Prevent movement in forward/backward (Z) and don't affect vertical motion (Y)
        velocity.z = 0; // lock forward/back movement
        rb.linearVelocity = velocity;

        if (isGrounded && moveHorizontal == 0)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }


    void Jump()
    {
        isGrounded = false;
        groundCheckTimer = groundCheckDelay;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

    void ApplyJumpPhysics()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime;
        }
    }

    bool CheckIfGrounded()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;

        // Debug ray for testing
        Debug.DrawRay(rayOrigin, Vector3.down * raycastDistance, Color.red);

        if (Physics.SphereCast(rayOrigin, 0.3f, Vector3.down, out RaycastHit hit, raycastDistance, groundLayer))
        {
            // Check that the surface is mostly flat
            return Vector3.Angle(hit.normal, Vector3.up) < 45f;
        }

        return false;
    }
}