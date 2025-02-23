using System.Collections.Generic;
using UnityEngine;

public class RewindObject : MonoBehaviour
{
    public bool isRewinding = false;
    public float maxTimeRecordInSeconds = 5;
    private List<TimeTick> timeTicks;
    private Rigidbody2D rb => PlayerManager.instance.playerMovement.RB;
    private Animator animator;
    private SpriteRenderer spriteRenderer => PlayerManager.instance.playerRenderer;
    private Color originalColor;
    private Color rewindColor = new Color(0.5f, 0.7f, 1f, 0.8f);

    void Start()
    {
        timeTicks = new List<TimeTick>();
        animator = GetComponent<Animator>();
        originalColor = spriteRenderer.color; 
    }

    void Update()
    {
        if (Input.GetButtonDown("Rewind"))
        {
            PlayerManager.instance.playerMovement.EnablePlayerMovement(false);
            StartRewind();
        }

        if (Input.GetButtonUp("Rewind"))
        {
            PlayerManager.instance.playerMovement.EnablePlayerMovement(true);
            StopRewind();
        }

        if (isRewinding)
        {
            float t = Mathf.PingPong(Time.time * 5f, 1f);
            spriteRenderer.color = Color.Lerp(originalColor, rewindColor, t);
        }
    }

    private void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            RecordTimeTick();
        }
    }

    private void RecordTimeTick()
    {
        timeTicks.Insert(0, new TimeTick(transform.position, transform.rotation, rb.velocity, rb.angularVelocity, GetCurrentAnimationHash()));

        if (timeTicks.Count > maxTimeRecordInSeconds * 100 && maxTimeRecordInSeconds > 0)
        {
            timeTicks.RemoveAt(timeTicks.Count - 1);
        }
    }

    public void StartRewind()
    {
        isRewinding = true;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        animator.enabled = false;
    }

    public void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
        animator.enabled = true;
        spriteRenderer.color = originalColor;
        RestoreAnimationState();
    }

    private void Rewind()
    {
        if (timeTicks.Count > 1)
        {
            TimeTick currentTick = timeTicks[0];
            TimeTick nextTick = timeTicks[1];

            rb.MovePosition(Vector3.Lerp(transform.position, nextTick.position, Time.fixedDeltaTime * 15));
            rb.MoveRotation(Quaternion.Lerp(transform.rotation, nextTick.quaternion, Time.fixedDeltaTime * 15));

            rb.velocity = nextTick.velocity;
            rb.angularVelocity = nextTick.angularVelocity;

            animator.Play(currentTick.animationStateHash, 0, 0);

            timeTicks.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }
    }

    private int GetCurrentAnimationHash()
    {
        return animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
    }

    private void RestoreAnimationState()
    {
        if (timeTicks.Count > 0)
        {
            animator.Play(timeTicks[0].animationStateHash, 0, 0);
        }
    }
}
