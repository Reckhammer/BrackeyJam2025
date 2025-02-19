using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public bool canMove = true;
    public float moveSpeed = 8f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    public float airControlMultiplier = 0.5f;
    private float moveInput;
    private float targetSpeed;
    private float speedDiff;
    private float accelRate;

    [Header("Jumping")]
    public bool canJump = true;
    public float jumpForce = 12f;
    public float jumpTimeMax = 0.3f;
    private float jumpTime;
    private bool isJumping;

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

    private Rigidbody2D RB;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        RB.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Update()
    {
        CheckGrounded();

        if (canMove)
            moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (canJump && jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            isJumping = true;
            jumpTime = jumpTimeMax;
            RB.velocity = new Vector2(RB.velocity.x, jumpForce);
            jumpBufferCounter = 0;
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
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        targetSpeed = moveInput * moveSpeed;
        speedDiff = targetSpeed - RB.velocity.x;
        accelRate = (isGrounded ? acceleration : acceleration * airControlMultiplier);
        float movement = Mathf.Lerp(RB.velocity.x, targetSpeed, accelRate * Time.fixedDeltaTime);
        RB.velocity = new Vector2(movement, RB.velocity.y);
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
    }
}
