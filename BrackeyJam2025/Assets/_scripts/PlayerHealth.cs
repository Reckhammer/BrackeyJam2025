using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event Action PlayerDied;

    private void PlayerDeath()
    {
        PlayerManager.instance.playerMovement.EnablePlayerMovement(false);
        PlayerManager.instance.playerMovement.RB.velocity = Vector2.zero;
        PlayerManager.instance.playerMovement.ClearPlayerInput();
        PlayerDied?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            PlayerDeath();
        }
    }
}
