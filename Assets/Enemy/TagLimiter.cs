using UnityEngine;

public class TagLimiter : MonoBehaviour
{
    public string targetTag = "Target"; // Set the tag to monitor.
    public int maxCount = 10;          // Maximum allowed objects with the tag.

    void Start()
    {
        // Optional: Add logic to enforce the limit when the game starts.
        EnforceTagLimit();
    }

    void Update()
    {
        // Continuously enforce the limit if needed.
        EnforceTagLimit();
    }

    void EnforceTagLimit()
    {
        // Find all objects with the specified tag.
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);

        // Check if the count exceeds the maximum allowed.
        if (taggedObjects.Length > maxCount)
        {
            for (int i = maxCount; i < taggedObjects.Length; i++)
            {
                Destroy(taggedObjects[i]); // Destroy the excess objects.
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Handle the case where an object with the tag enters a monitored area.
        if (other.CompareTag(targetTag))
        {
            EnforceTagLimit();
        }
    }
}
