using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public bool canMove = true;
    public float moveSpeed = 8f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    public float airControlMultiplier = 0.5f;
    private float xInput;
    private float yInput;
    private float targetSpeed;
    private float speedDiff;
    private float accelRate;

    [Header("Jumping")]
    public bool canJump = true;
    public float jumpForce = 12f;
    public float jumpTimeMax = 0.3f;
    public int maxMultiJumps = 1;
    private int multiJumpCounter;
    private float jumpTime;
    private bool isJumping;
    public event Action PlayerJumpStarted;
    public event Action PlayerLanded;

    [Header("Jump Assistance")]
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;


    [Header("Ground Check")]
    public Transform groundCheck;
    public bool isGrounded { get; private set; }
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Dash")]
    public bool canDash = true;
    public float dashForce = 200f;
    public float dashCooldownTime = 1f;
    private float timeLastDashed = 0f;

    public Rigidbody2D RB { get; private set; }
    private SpriteRenderer SR => PlayerManager.instance.playerRenderer;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        RB.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Update()
    {
        CheckGrounded();

        if (canMove)
        {
            xInput = Input.GetAxisRaw("Horizontal");
            yInput = Input.GetAxisRaw("Vertical");
        }

        // Flip the sprite depending on the x input direction
        HandleSpriteDirection();

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (canJump && jumpBufferCounter > 0 && (coyoteTimeCounter > 0 || multiJumpCounter < maxMultiJumps))
        {
            //Debug.Log("Applying Jump Force");
            isJumping = true;
            jumpTime = jumpTimeMax;
            RB.velocity = new Vector2(RB.velocity.x, jumpForce);
            jumpBufferCounter = 0;
            multiJumpCounter++;

            PlayerJumpStarted?.Invoke();
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTime > 0)
            {
                RB.velocity = new Vector2(RB.velocity.x, jumpForce);
                jumpTime -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            if (RB.velocity.y > 0)
            {
                RB.velocity = new Vector2(RB.velocity.x, RB.velocity.y * 0.5f);
            }
        }

        if (canDash && Input.GetButtonDown("Dash"))
            HandleDash();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        targetSpeed = xInput * moveSpeed;
        speedDiff = targetSpeed - RB.velocity.x;
        accelRate = (isGrounded ? acceleration : acceleration * airControlMultiplier);
        float movement = Mathf.Lerp(RB.velocity.x, targetSpeed, accelRate * Time.fixedDeltaTime);
        RB.velocity = new Vector2(movement, RB.velocity.y);
    }

    private void CheckGrounded()
    {
        bool prevGroundedState = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            multiJumpCounter = 0;

            if (prevGroundedState == false)
                PlayerLanded?.Invoke();
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void HandleDash()
    {
        if (Time.time - timeLastDashed > dashCooldownTime)
        {
            Vector2 dashMoveDirection;

            if (xInput == 0f && yInput == 0f)
                dashMoveDirection = SR.flipX ? Vector2.left : Vector2.right;
            else
                dashMoveDirection = new Vector2(xInput, yInput).normalized;

            RB.AddForce(dashMoveDirection * dashForce);
            timeLastDashed = Time.time;
        }
    }

    private void HandleSpriteDirection()
    {
        // if xInput is positive, moving right so look right
        // if xInput is negative, moving left so look left
        // Don't do anything if no xInput
        if (xInput > 0)
            SR.flipX = false;
        else if (xInput < 0)
            SR.flipX = true;
    }

    public void EnablePlayerMovement(bool enable)
    {
        canMove = enable;
        canJump = enable;
        canDash = enable;
    }

    public void ClearPlayerInput() //No slideeeeeee
    {
        xInput = 0f;
        yInput = 0f;
    }
}
