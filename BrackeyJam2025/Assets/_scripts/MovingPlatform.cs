using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MovingPlatform : MonoBehaviour
{
    public Vector3 endPosition;
    private Vector3 startPosition;
    private Vector3 platformVelocity;
    private Vector3 previousPosition;
    public float travelTime = 3f;
    public float waitTime = 1f;
    public bool loopTravel = true;
    public bool travelOnStart = true;

    public bool parentOnCollision = false;
    private List<Rigidbody2D> objectsOnPlatform2d = new List<Rigidbody2D>();
    private List<Rigidbody> objectsOnPlatform3d = new List<Rigidbody>();

    private void Start()
    {
        startPosition = transform.position;

        if (travelOnStart)
            StartTraveling();
    }

    private void Update()
    {
        // Calculate platform velocity
        platformVelocity = transform.position - previousPosition;
        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // Add platform velocity to children so they move with platform
        

        // Apply velocity to objects standing on the platform
        foreach (Rigidbody2D rb in objectsOnPlatform2d)
        {
            // Ensure the object hasn't been destroyed or removed
            if (rb != null)
                rb.velocity += (Vector2) platformVelocity;
        }

        foreach (Rigidbody rb in objectsOnPlatform3d)
        {
            if (rb != null)
                rb.velocity += platformVelocity;
        }
    }

    public void StartTraveling()
    {
        StartCoroutine(TravelCoroutine());
    }

    private IEnumerator TravelCoroutine()
    {
        do
        {
            yield return MoveToPosition(startPosition, endPosition);
            yield return new WaitForSeconds(waitTime);
            yield return MoveToPosition(endPosition, startPosition);
            yield return new WaitForSeconds(waitTime);
        } 
        while (loopTravel);
    }

    private IEnumerator MoveToPosition(Vector3 start, Vector3 end)
    {
        float elapsedTime = 0f;

        while (elapsedTime < travelTime)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime / travelTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end;  // Ensure exact positioning
    }

    #region Collision

    private void OnCollisionEnter(Collision collision)
    {
        if (parentOnCollision)
        {
            if (collision.gameObject.TryGetComponent(out Rigidbody rb))
            {
                if (!objectsOnPlatform3d.Contains(rb))
                    objectsOnPlatform3d.Add(rb);
            }

            //collision.transform.SetParent(this.transform, true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (parentOnCollision)
        {
            if (collision.gameObject.TryGetComponent(out Rigidbody rb))
            {
                objectsOnPlatform3d.Remove(rb);
            }

            //collision.transform.SetParent(null);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (parentOnCollision)
        {
            if (collision.gameObject.TryGetComponent(out Rigidbody2D rb))
            {
                if (!objectsOnPlatform2d.Contains(rb))
                    objectsOnPlatform2d.Add(rb);
            }

            //collision.transform.SetParent(this.transform, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (parentOnCollision)
        {
            if (collision.gameObject.TryGetComponent(out Rigidbody2D rb))
            {
                objectsOnPlatform2d.Remove(rb);
            }

            //collision.transform.SetParent(null);
        }
    }

    #endregion

    #region Editor_Funcs
#if UNITY_EDITOR

    [ContextMenu("Set End Position to Start")]
    public void SetEndPosToStartPos()
    {
        Undo.RegisterCompleteObjectUndo(this, "Set End Pos to Start");

        endPosition = transform.position;

        EditorUtility.SetDirty(this);
    }

#endif
    #endregion

}
