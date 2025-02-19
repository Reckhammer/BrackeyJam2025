using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector2 offset;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = new(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
