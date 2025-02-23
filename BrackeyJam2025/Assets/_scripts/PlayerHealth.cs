// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerHealth : MonoBehaviour
// {
//     public event Action PlayerDied;

//     private void PlayerDeath()
//     {
//         PlayerManager.instance.playerMovement.EnablePlayerMovement(false);
//         PlayerManager.instance.playerMovement.RB.velocity = Vector2.zero;
//         PlayerManager.instance.playerMovement.ClearPlayerInput();
//         PlayerDied?.Invoke();
//     }

//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.layer == LayerMask.NameToLayer("Death"))
//         {
//             PlayerDeath();
//         }
//     }
// }



using System;
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
        PlayerDied?.Invoke();

        PlayerManager.instance.playerMovement.EnablePlayerMovement(false);
        // PlayerManager.instance.playerMovement.RB.velocity = Vector2.zero;
        PlayerManager.instance.playerMovement.ClearPlayerInput();
        PlayerManager.instance.playerMovement.canMove = false;
        PlayerManager.instance.playerMovement.canJump = false;
        PlayerManager.instance.playerMovement.canDash = false;

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

