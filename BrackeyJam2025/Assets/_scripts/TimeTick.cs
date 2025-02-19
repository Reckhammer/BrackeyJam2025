using UnityEngine;

public class TimeTick : MonoBehaviour
{
    public Vector3 position;
    public Quaternion quaternion;
    public Vector3 velocity;
    public float angularVelocity;

    public TimeTick (Vector3 pos, Quaternion rot, Vector3 vel, float angVel)
    {
        position = pos;
        quaternion = rot;
        velocity = vel;
        angularVelocity = angVel;
    }
}
