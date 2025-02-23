using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerMovement = PlayerManager.instance.playerMovement;

        playerMovement.PlayerJumpStarted += PlayerJumpStarted;
        playerMovement.PlayerLanded += PlayerLanded;

        playerHealth = PlayerManager.instance.playerHealth;
        playerHealth.PlayerDied += PlayerDeath;
        PlayerManager.instance.PlayerRespawned += PlayerRespawned;
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

    private void PlayerDeath()
    {
        animator.SetTrigger("IsDead");
    }

    private void PlayerRespawned()
    {
        animator.SetTrigger("IsRespawned");
    }
}
