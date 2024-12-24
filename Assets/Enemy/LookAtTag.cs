using UnityEngine;

public class LookAtTag : MonoBehaviour
{
    public string targetTag = "Target"; // Set the tag of the objects you want to look at.

    void Update()
    {
        GameObject closestTarget = FindClosestWithTag(targetTag);
        if (closestTarget != null)
        {
            // Rotate the object to look at the closest target
            Vector3 direction = closestTarget.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Smooth rotation
        }
    }

    GameObject FindClosestWithTag(string tag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < minDistance)
            {
                closest = target;
                minDistance = distance;
            }
        }

        return closest;
    }
}
