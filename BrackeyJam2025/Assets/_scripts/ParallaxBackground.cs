using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Camera camera;
    public float parallaxEffectMultiplier = 0.5f;

    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;

    void Start()
    {
        lastCameraPosition = camera.transform.position;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        textureUnitSizeX = spriteRenderer.bounds.size.x;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = camera.transform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, 0, 0);
        lastCameraPosition = camera.transform.position;

        if (camera.transform.position.x - transform.position.x >= textureUnitSizeX)
        {
            transform.position += new Vector3(textureUnitSizeX * 2f, 0, 0);
        }
        else if (transform.position.x - camera.transform.position.x >= textureUnitSizeX)
        {
            transform.position -= new Vector3(textureUnitSizeX * 2f, 0, 0);
        }
    }
}
