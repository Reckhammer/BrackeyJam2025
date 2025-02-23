using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public bool isDead { get; private set; } = false;
    public event Action PlayerDied;
    public event Action PlayerRevived;
    private RewindObject rewindObject;

    private void Start()
    {
        rewindObject = GetComponent<RewindObject>();
    }

    private void PlayerDeath()
    {
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        // Get Animation triggered
        PlayerDied?.Invoke();
        isDead = true;
        PlayerManager.instance.playerMovement.EnablePlayerMovement(false);
        PlayerManager.instance.playerMovement.ClearPlayerInput();

        yield return new WaitForSeconds(2f);

        // Trigger Normal animation
        PlayerRevived?.Invoke();

        yield return new WaitForSeconds(1f);

        rewindObject.EnterRewindSelection();

        UIManager.Instance.ShowRewindUI();

    }

    public void PlayerRevive()
    {
        isDead = false;
        PlayerManager.instance.playerMovement.EnablePlayerMovement(true);
        PlayerRevived?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            PlayerDeath();
        }
    }
}

