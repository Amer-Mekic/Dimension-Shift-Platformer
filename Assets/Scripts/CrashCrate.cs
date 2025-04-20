using UnityEngine;

public class Crate : MonoBehaviour
{
    public float fallHeightThreshold = 3f;
    public float moveCooldown = 0.3f;
    public float raycastDistance = 0.55f; // Slightly more than half the box height
    private float highestPoint;
    private float lastMoveTime;
    private bool isGrounded = false;
    private bool isFalling = false;
    private Vector3 boxSize;

    [Header("Crate Components")]
    public BoxCollider boxCollider;
    public MeshRenderer wholeCrate;
    public GameObject fracturedCrate;
    public AudioSource crashAudioClip;

    private void Start()
    {
        boxSize = boxCollider.size;
        GroundCheck(true); // Force check on start

        if (!isGrounded)
        {
            isFalling = true;
            highestPoint = transform.position.y;
        }
    }

    private void Update()
    {
        GroundCheck();

        if (isFalling)
        {
            transform.position += Vector3.down * 9.81f * Time.deltaTime;

            if (isGrounded)
            {
                float fallDistance = highestPoint - transform.position.y;
                if (fallDistance >= fallHeightThreshold)
                {
                    BreakCrate();
                }
                isFalling = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.time - lastMoveTime < moveCooldown) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 direction = collision.contacts[0].point - transform.position;
            direction = direction.normalized;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                float step = direction.x > 0 ? -1 : 1;
                Vector3 targetPos = transform.position + new Vector3(step, 0, 0);

                if (IsMoveValid(targetPos))
                {
                    transform.position = targetPos;
                    lastMoveTime = Time.time;
                    GroundCheck(true); // Force update
                }
            }
        }
    }

    private void GroundCheck(bool force = false)
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.down * (boxSize.y / 2f - 0.01f); // Slight offset
        isGrounded = Physics.BoxCast(origin, boxSize / 2f * 0.95f, Vector3.down, out hit, Quaternion.identity, raycastDistance);

        if (!isGrounded && !isFalling || force && !isGrounded)
        {
            highestPoint = transform.position.y;
            isFalling = true;
        }
    }

    private bool IsMoveValid(Vector3 targetPos)
    {
        // Prevent moving into empty space
        Vector3 origin = targetPos + Vector3.down * (boxSize.y / 2f - 0.01f);
        return Physics.BoxCast(origin, boxSize / 2f * 0.95f, Vector3.down, out _, Quaternion.identity, raycastDistance);
    }

    private void BreakCrate()
    {
        wholeCrate.enabled = false;
        boxCollider.enabled = false;
        fracturedCrate.SetActive(true);
        crashAudioClip.Play();
        Destroy(gameObject, 2f);
    }

    [ContextMenu("Test Break")]
    public void TestBreak()
    {
        BreakCrate();
    }
}