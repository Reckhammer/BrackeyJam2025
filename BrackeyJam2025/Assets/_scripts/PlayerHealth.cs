using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event Action PlayerDied;
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
        PlayerManager.instance.playerMovement.EnablePlayerMovement(false);
        PlayerManager.instance.playerMovement.ClearPlayerInput();

        yield return new WaitForSeconds(2f);

        // Trigger Normal animation
        PlayerManager.instance.PlayerRespawn();

        yield return new WaitForSeconds(1f);

        rewindObject.EnterRewindSelection();

        UIManager.Instance.ShowRewindUI();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            PlayerDeath();
        }
    }
}

