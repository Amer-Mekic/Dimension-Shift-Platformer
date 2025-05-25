using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private bool isGrounded = true; // Simple grounded check for jump logic

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Movement detection
        bool isMoving = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
        animator.SetBool("isRunning", isMoving);

        // Jump detection
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.SetBool("isJumping", true);
            isGrounded = false; // Prevent multiple jumps
        }
    }

    // Simulate landing (you can replace this with real collision logic)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }
}
