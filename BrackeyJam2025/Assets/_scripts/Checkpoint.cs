using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Implement Giving up rewind and respawning at mushroom
    public bool isDefaultCheckpoint = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //if (isDefaultCheckpoint)
        //    CheckpointManager.instance.SetCheckpoint(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            //CheckpointManager.instance.SetCheckpoint(this);

            if (animator != null)
                animator.SetTrigger("Activate");
        }
    }
}
