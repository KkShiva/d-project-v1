using UnityEngine;

public class DestroyHandler : MonoBehaviour
{
    [Header("Timers")]
    [Tooltip("Enable to destroy after a given time.")]
    public bool destroyAfterTime = false;
    [Tooltip("Time after which the object will be destroyed.")]
    public float destroyTime = 5f;

    [Header("Collision Settings")]
    [Tooltip("Enable to destroy on collision with a specific layer.")]
    public bool destroyOnSpecificLayer = false;
    [Tooltip("The specific layer to check for collision.")]
    public LayerMask targetLayer;

    [Tooltip("Enable to destroy on collision with any layer.")]
    public bool destroyOnAnyCollision = false;

    [Header("Timed Layer Collision Settings")]
    [Tooltip("Enable to destroy after a given time when colliding with a specific layer.")]
    public bool destroyOnSpecificLayerAfterTime = false;
    [Tooltip("Time delay before destruction after colliding with the specific layer.")]
    public float delayAfterLayerCollision = 3f;

    private bool isDestroyScheduled = false; // Prevents multiple destruction calls for the same collision

    private void Start()
    {
        // Start the timer if destroyAfterTime is enabled
        if (destroyAfterTime)
        {
            Destroy(gameObject, destroyTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Destroy immediately on collision with a specific layer
        if (destroyOnSpecificLayer && (targetLayer == (targetLayer | (1 << collision.gameObject.layer))))
        {
            Destroy(gameObject);
        }

        // Destroy immediately on collision with any layer
        if (destroyOnAnyCollision)
        {
            Destroy(gameObject);
        }

        // Destroy after a delay if colliding with a specific layer
        if (destroyOnSpecificLayerAfterTime && !isDestroyScheduled &&
            (targetLayer == (targetLayer | (1 << collision.gameObject.layer))))
        {
            isDestroyScheduled = true;
            StartCoroutine(DestroyAfterDelay(delayAfterLayerCollision));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the object has a trigger collider, similar checks can be applied
        if (destroyOnSpecificLayer && (targetLayer == (targetLayer | (1 << other.gameObject.layer))))
        {
            Destroy(gameObject);
        }

        if (destroyOnAnyCollision)
        {
            Destroy(gameObject);
        }

        if (destroyOnSpecificLayerAfterTime && !isDestroyScheduled &&
            (targetLayer == (targetLayer | (1 << other.gameObject.layer))))
        {
            isDestroyScheduled = true;
            StartCoroutine(DestroyAfterDelay(delayAfterLayerCollision));
        }
    }

    private System.Collections.IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
