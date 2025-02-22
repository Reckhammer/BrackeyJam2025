using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        playerMovement.PlayerJumpStarted += PlayerJumpStarted;
        playerMovement.PlayerLanded += PlayerLanded;
    }

    void Update()
    {
        float speed = Mathf.Abs(Input.GetAxis("Horizontal"));
        animator.SetFloat("Speed", speed);
    }

    private void PlayerJumpStarted()
    {
        animator.ResetTrigger("JumpLandTrigger");
        animator.SetTrigger("JumpTrigger");
    }

    private void PlayerLanded()
    {
        animator.ResetTrigger("JumpTrigger");
        animator.SetTrigger("JumpLandTrigger");
    }
}
