using UnityEngine;

public class LookSameDirection : MonoBehaviour
{
    // Reference to the object to align with
    public Transform targetObject;

    void Update()
    {
        // Ensure the targetObject is assigned
        if (targetObject != null)
        {
            // Set this object's forward direction to match the target's forward direction
            transform.rotation = targetObject.rotation;
        }
        else
        {
            Debug.LogWarning("Target object is not assigned.");
        }
    }
}
