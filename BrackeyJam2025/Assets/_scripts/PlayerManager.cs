using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public SpriteRenderer playerRenderer;
    public PlayerMovement playerMovement;
    public Camera playerCamera;
    public PlayerHealth playerHealth;
    public Collider2D playerCollider;

    public event Action PlayerRespawned;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }
    
    public void PlayerRespawn()
    {
        playerMovement.EnablePlayerMovement(true);
        PlayerRespawned?.Invoke();
    }
}
