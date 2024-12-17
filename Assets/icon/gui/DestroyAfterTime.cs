using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float destroyTime = 5f;  // Time in seconds before the object is destroyed

    private void Start()
    {
        // Destroy this GameObject after the specified time
        Destroy(gameObject, destroyTime);
    }
}
