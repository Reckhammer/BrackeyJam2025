using System.Collections.Generic;
using UnityEngine;

public class RewindObject : MonoBehaviour
{
    public bool isRewinding = false;
    public float maxTimeRecordInSeconds = 5;
    List<TimeTick> timeTicks;
    Rigidbody2D rb;

    private void Start() 
    {
        timeTicks = new List<TimeTick>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() 
    {
        timeTicks.Insert(0, new TimeTick(transform.position,transform. rotation, rb.velocity, rb.angularVelocity));

        if (timeTicks.Count > maxTimeRecordInSeconds * 100)
        {
            timeTicks.RemoveAt(timeTicks.Count - 1);
        }
    }
    public void Rewind()
    {
        if (timeTicks.Count > 0)
        {
            TimeTick tick = timeTicks[0];
            transform.position = tick.position;
            transform.rotation = tick.quaternion;
            rb.velocity = tick.velocity;
            rb.angularVelocity = tick.angularVelocity;
            timeTicks.RemoveAt(0);
        }
    }
}
