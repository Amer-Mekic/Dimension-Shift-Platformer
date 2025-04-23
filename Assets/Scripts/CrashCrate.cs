using UnityEngine;

public class Crate : MonoBehaviour
{
    [Header("Crate Settings")]
    public float moveCooldown = 0f;               // Time before the crate can be moved again
    public float raycastDistance = 0.497f;            // Distance for ground detection
    public float fallHeightThreshold = 2.99f;           // Height at which the crate breaks when falling

    private float lastMoveTime;
    private float highestPoint;
    private bool isGrounded = false;
    private bool isFalling = false;
    private bool isBroken = false;  // Track if the crate is already broken

    [Header("Crate Components")]
    public BoxCollider boxCollider;                 // BoxCollider for the crate
    public MeshRenderer wholeCrate;                 // Renderer for the crate
    public GameObject fracturedCrate;               // Object for the fractured crate when it breaks
    public AudioSource crashAudioClip;              // Audio clip for the crash sound

    private void Start()
    {
        Debug.Log("[Start] Crate initialization");
        GroundCheck();  // Force initial ground check

        if (!isGrounded)
        {
            Debug.Log("[Start] Crate not grounded. Starting fall.");
            isFalling = true;
            highestPoint = transform.position.y;
        }
        else
        {
            Debug.Log("[Start] Crate is grounded.");
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();

        // If the crate is falling, make it fall down
        if (isFalling && !isBroken)
        {
            transform.position += Vector3.down * 10f * Time.deltaTime;
            Debug.Log("[Update] Crate falling...");

            // Ensure fall logic checks the ground on each frame
            if (isGrounded)
            {
                float fallDistance = highestPoint - transform.position.y;
                Debug.Log($"[Update] Crate landed. Fall distance: {fallDistance}");

                if (fallDistance >= fallHeightThreshold && !isBroken)
                {
                    Debug.Log("[Update] Fall threshold exceeded. Breaking crate.");
                    BreakCrate();
                }
                else
                {
                    Debug.Log("[Update] Fall distance below threshold. Crate landed safely.");
                }

                isFalling = false;
            }
        }
    }

    public void MoveCrate(Vector3 direction)
    {
        if (Time.time - lastMoveTime < moveCooldown || isBroken) return;

        Vector3 targetPos = transform.position + direction;
        if (IsMoveValid(targetPos)) // Check if the move is valid (no collision or ground detected)
        {
            transform.position = targetPos;
            lastMoveTime = Time.time;
            GroundCheck(); // Force ground check after the move
            Debug.Log($"[Crate] Moved to new position: {targetPos}");
        }
        else
        {
            Debug.Log("[Crate] Invalid move, cannot move to the target position.");
        }
    }

    private void GroundCheck()
{
    Bounds bounds = boxCollider.bounds;
    Vector3 origin = bounds.center;
    Vector3 halfExtents = bounds.extents * 0.95f;

    RaycastHit hit;
    bool previousGrounded = isGrounded;
    isGrounded = Physics.BoxCast(origin, halfExtents, Vector3.down, out hit, Quaternion.identity, raycastDistance);

    Debug.DrawRay(origin, Vector3.down * raycastDistance, isGrounded ? Color.green : Color.red);

    // Add tolerance to ground detection for raycast
    if (isGrounded && Mathf.Abs(hit.point.y - transform.position.y) < 0.3f)
    {
        transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z); // Snap to ground level
    }

    if (!previousGrounded && isGrounded)
    {
        Debug.Log("[GroundCheck] Crate has landed.");
    }

    if ((!isGrounded && !isFalling))
    {
        Debug.Log("[GroundCheck] No ground detected. Starting fall.");
        highestPoint = transform.position.y;
        isFalling = true;
    }
}


    private bool IsMoveValid(Vector3 targetPos)
    {
        Bounds bounds = boxCollider.bounds;
        Vector3 moveDirection = (targetPos - transform.position).normalized;
        float moveDistance = Vector3.Distance(transform.position, targetPos);

        // Obstacle raycast (box-shaped) in the movement direction
        Vector3 origin = bounds.center;
        Vector3 halfExtents = bounds.extents * 0.95f;
        Quaternion orientation = Quaternion.identity;

        bool obstacleHit = Physics.BoxCast(origin, halfExtents, moveDirection, out RaycastHit hit, orientation, moveDistance);
        
        if (obstacleHit)
        {
            Debug.Log($"[IsMoveValid] Obstacle detected: {hit.collider.gameObject.name}");
            return false;
        }

        // Check for ground directly beneath the target position
        Vector3 groundCheckOrigin = targetPos + Vector3.up * bounds.extents.y;
        bool isGrounded = Physics.BoxCast(groundCheckOrigin, halfExtents, Vector3.down, out _, Quaternion.identity, raycastDistance);

        bool isEdge = IsEdge(targetPos);

        if (isGrounded || isEdge)
        {
            Debug.DrawRay(groundCheckOrigin, Vector3.down * raycastDistance, isGrounded ? Color.green : Color.yellow);
            return true;
        }
        else
        {
            Debug.DrawRay(groundCheckOrigin, Vector3.down * raycastDistance, Color.red);
            return false;
        }
    }

    private bool IsEdge(Vector3 targetPos)
    {
        // Check if the crate is near the edge by raycasting from the edges of the target position
        Vector3 checkOrigin = targetPos + new Vector3(0, 0.1f, 0); 
        RaycastHit hit;

        // Perform a raycast directly downward from a point just above the crate
        bool isEdge = !Physics.Raycast(checkOrigin, Vector3.down, out hit, raycastDistance);

        // Debug ray for edge detection
        Debug.DrawRay(checkOrigin, Vector3.down * raycastDistance, isEdge ? Color.yellow : Color.red);
        Debug.Log($"[IsEdge] Is the crate at the edge? {isEdge}");

        return isEdge;
    }

    private void BreakCrate()
    {
        if (isBroken) return;  // Prevent further break triggers, break once only

        Debug.Log("[BreakCrate] Crate breaking logic triggered.");
        wholeCrate.enabled = false;  // Disable the intact crate
        boxCollider.enabled = false;  // Disable the collider
        fracturedCrate.SetActive(true);  // Show the fractured crate
        crashAudioClip.Play();  // Play the crash sound
        isBroken = true;  // Set the crate as broken to avoid repeating the process
        Destroy(gameObject, 1f);  // Destroy the original crate after 1 second
    }

    [ContextMenu("Test Break")]
    public void TestBreak()
    {
        BreakCrate();
    }

    private void OnDrawGizmosSelected() // helper fun. for debugging (draws cyan box that detects ground)
    {
        if (boxCollider == null) return;

        Bounds bounds = boxCollider.bounds;
        Vector3 origin = bounds.center;
        Vector3 halfExtents = bounds.extents * 0.95f;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(origin + Vector3.down * raycastDistance, halfExtents * 2);
    }
}
