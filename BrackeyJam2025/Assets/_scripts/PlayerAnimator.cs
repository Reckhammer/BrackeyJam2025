using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private bool wasInAir = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        float speed = Mathf.Abs(Input.GetAxis("Horizontal"));
        animator.SetFloat("Speed", speed);

        if (Input.GetButtonDown("Jump") && playerMovement.isGrounded)
        {
            Debug.Log("Jump Trigger");
            animator.SetTrigger("JumpTrigger");
            wasInAir = true;
        }

        if (!playerMovement.isGrounded && rb.velocity.y >= 0)
        {
            animator.SetBool("IsJumping", true);
        }

        if (playerMovement.isGrounded && wasInAir)
        {
            animator.SetTrigger("JumpLandTrigger");
            wasInAir = false;
        }

        if (playerMovement.isGrounded)
        {
            animator.ResetTrigger("JumpTrigger");
            animator.SetBool("IsJumping", false);
        }
    }
}
