using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public bool canMove = true;
    public float moveSpeed = 5f;
    private float xInput = 0f;

    [Header("Jump")]
    public bool  canJump = true;
    public float jumpForce = 100f;
    public bool  isGrounded { get; private set; }
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    
    private Rigidbody2D RB;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckGrounded();

        //Player 'A', 'D', right arrow, left arrow, and remote controller stick movement
        if (canMove)
            xInput = Input.GetAxis("Horizontal");
        
        if (canJump && Input.GetButtonDown("Jump"))
            PlayerJump();
    }

    private void FixedUpdate()
    {
        RB.velocity = new Vector2(xInput * moveSpeed, RB.velocity.y);
    }

    private void PlayerJump()
    {
        if (isGrounded)
        {
            RB.AddForce(new Vector2(0, jumpForce));
        }
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
