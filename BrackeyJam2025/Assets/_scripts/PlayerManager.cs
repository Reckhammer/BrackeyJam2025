using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public SpriteRenderer playerRenderer;
    public PlayerMovement playerMovement;
    public Camera playerCamera;
    public PlayerHealth playerHealth;
    public Collider2D playerCollider;
    public Animator animator;
    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }

#if UNITY_EDITOR
    [ContextMenu("Fill Player Refs")]
    public void FillPlayerRefs()
    {
        Undo.RegisterCompleteObjectUndo(this, "Fill Player Refs");

        PlayerMovement playerRef = GameObject.FindObjectOfType<PlayerMovement>();

        if (playerRef != null)
        {
            playerMovement = playerRef;
            playerRenderer = playerRef.GetComponent<SpriteRenderer>();
            playerHealth = playerRef.GetComponent<PlayerHealth>();
            playerCollider = playerRef.GetComponent<Collider2D>();
            animator = playerRef.GetComponent<Animator>();
        }

        playerCamera = GameObject.FindObjectOfType<Camera>();

        EditorUtility.SetDirty(this);
    }
#endif
}
