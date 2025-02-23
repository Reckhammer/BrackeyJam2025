using System.Collections.Generic;
using UnityEngine;

public class RewindObject : MonoBehaviour
{
    public bool isRewinding = false;
    public float maxTimeRecordInSeconds = 5;
    private List<TimeTick> timeTicks;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Color rewindColor = new Color(0.5f, 0.7f, 1f, 0.8f);
    
    private bool isSelectingRewind = false;
    public float rewindSpeed = 3f;
    public int rewindIndex = 0;

    void Start()
    {
        timeTicks = new List<TimeTick>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; 
        RewindManager.Instance.RegisterRewindObject(this);
    }

    void Update()
    {
        if (isRewinding || isSelectingRewind)
        {
            float t = Mathf.PingPong(Time.time * 5f, 1f);
            spriteRenderer.color = Color.Lerp(originalColor, rewindColor, t);
        }
        if (isSelectingRewind)
        {
            HandleRewindSelection();
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRewind();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            StopRewind();
        }
    }

    private void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else if (!isSelectingRewind)
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

    public void PauseRewindRecording()
    {
        isRewinding = false;
    }

    public void EnterRewindSelection()
    {
        isSelectingRewind = true;
        rewindIndex = 0;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        animator.enabled = false;
    }

    private void HandleRewindSelection()
    {
        float currentSpeed = rewindSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.R) && rewindIndex < timeTicks.Count - 1)
        {
            rewindIndex = Mathf.Clamp(
                Mathf.RoundToInt(Mathf.Lerp(rewindIndex, timeTicks.Count - 1, currentSpeed)), 
                0, timeTicks.Count - 1
            );
            PreviewRewindState(rewindIndex);
            UIManager.Instance.rewindGauge.UseRewind(Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.F) && rewindIndex > 0)
        {
            rewindIndex = Mathf.Clamp(
                Mathf.RoundToInt(Mathf.Lerp(rewindIndex, 0, currentSpeed)), 
                0, timeTicks.Count - 1
            );
            PreviewRewindState(rewindIndex);
            UIManager.Instance.rewindGauge.RefundRewind(Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmRewindState(rewindIndex);
            isSelectingRewind = false;
        }
    }

    private void PreviewRewindState(int index)
    {
        if (timeTicks.Count == 0 || index >= timeTicks.Count) return;

        float lerpSpeed = Time.deltaTime * rewindSpeed;
        transform.position = Vector3.Lerp(transform.position, timeTicks[index].position, lerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, timeTicks[index].quaternion, lerpSpeed);
    }


    private void ConfirmRewindState(int index)
    {
        if (index < timeTicks.Count)
        {
            transform.position = timeTicks[index].position;
            transform.rotation = timeTicks[index].quaternion;
            rb.isKinematic = false;
            animator.enabled = true;
            UIManager.Instance.HideRewindUI();
            PlayerManager.instance.playerMovement.canMove = true;
            PlayerManager.instance.playerMovement.canJump = true;
            PlayerManager.instance.playerMovement.canDash = true;
        }
    }

    public int GetRewindTickCount()
    {
        return timeTicks.Count;
    }
}
