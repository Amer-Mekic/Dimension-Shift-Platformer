using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator; // Drag PlayerMesh's Animator here
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private bool isGrounded;

    void Update()
    {
        // Check movement
        bool isMoving = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
        animator.SetBool("isRunning", isMoving);

        // Ground check using raycast or overlap sphere
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.SetBool("isJumping", true);
        }

        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
        }
    }
}
