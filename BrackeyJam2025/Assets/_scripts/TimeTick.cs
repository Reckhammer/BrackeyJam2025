using UnityEngine;

public class TimeTick
{
    public Vector3 position;
    public Quaternion quaternion;
    public Vector3 velocity;
    public float angularVelocity;
    public int animationStateHash;

    public TimeTick(Vector3 pos, Quaternion rot, Vector3 vel, float angVel, int animState)
    {
        position = pos;
        quaternion = rot;
        velocity = vel;
        angularVelocity = angVel;
        animationStateHash = animState;
    }
}
