using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector2 moveDir = Vector2.left;
    private bool isMovingRight = false;
    public Transform groundCheck;
    public float groundCheckDistance = .2f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (AboutToFallOff())
            ReverseDirection();

        Debug.Log($"Move Dir: {moveDir}");
        rb.velocity = moveDir * moveSpeed;
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    private bool AboutToFallOff()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance);

        if (hit.collider == null)
            return true;

        return false;
    }

    private void ReverseDirection()
    {
        Debug.Log("Reversing directions");
        if (isMovingRight)
        {
            // Now moving left
            moveDir = Vector2.left;
            sr.flipX = false;
            groundCheck.localPosition = new Vector3(groundCheck.localPosition.x * -1, groundCheck.localPosition.y, groundCheck.localPosition.z);
            isMovingRight = false;
        }
        else
        {
            // Now moving right
            moveDir = Vector2.right;
            sr.flipX = true;
            groundCheck.localPosition = new Vector3(groundCheck.localPosition.x * -1, groundCheck.localPosition.y, groundCheck.localPosition.z);
            isMovingRight = true;
        }
    }
}
