using UnityEngine;

public class Crate : MonoBehaviour
{
    public float fallHeightThreshold = 3f;
    public float raycastDistance = 1f;
    public float moveCooldown = 0.5f;
    private float highestPoint;
    private float lastMoveTime;
    private bool isGrounded = false;
    private bool isFalling = false;

    [Header("Crate Components")]
    public BoxCollider boxCollider;
    public MeshRenderer wholeCrate;
    public GameObject fracturedCrate;
    public AudioSource crashAudioClip;

    private void Start()
    {
        GroundCheck(); // Check if it's grounded initially
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
                    BreakCrate();

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
                if (IsGroundBelow(targetPos)) // Optional: check if valid spot
                {
                    transform.position = targetPos;
                    lastMoveTime = Time.time;
                    GroundCheck(); // Refresh fall status
                }
            }
        }
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance);

        if (!isGrounded && !isFalling)
        {
            highestPoint = transform.position.y;
            isFalling = true;
        }
    }

    private bool IsGroundBelow(Vector3 pos)
    {
        return Physics.Raycast(pos, Vector3.down, raycastDistance);
    }

    private void BreakCrate()
    {
        wholeCrate.enabled = false;
        boxCollider.enabled = false;
        fracturedCrate.SetActive(true);
        crashAudioClip.Play();
        Destroy(gameObject, 2f);
    }
}