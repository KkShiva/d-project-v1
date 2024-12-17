using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    public float minDistanceFromGround = 20f; // Minimum desired distance from the ground
    public float maxYValue = 200f;            // Maximum Y height limit
    public LayerMask groundLayer;             // Layer mask for the ground

    public float heightAdjustmentSpeed = 5f;  // Speed at which the plane adjusts its height

    void Update()
    {
        // Adjust the height to avoid getting closer than the desired distance from the ground
        AvoidGettingTooCloseToGround();
    }

    void AvoidGettingTooCloseToGround()
    {
        RaycastHit hit;
        // Cast a ray downwards from the object's position
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            // Check the distance between the object and the ground
            float currentDistance = hit.distance;

            // If the object is closer than the minimum distance, adjust its height
            if (currentDistance < minDistanceFromGround)
            {
                // Calculate how much the object needs to rise to avoid getting too close
                float heightAdjustment = minDistanceFromGround - currentDistance;

                // Gradually adjust the height (with height adjustment speed for smooth motion)
                float newYPosition = transform.position.y + heightAdjustment * Time.deltaTime * heightAdjustmentSpeed;

                // Clamp the Y position to prevent exceeding the max height (200) or going too close to the ground
                newYPosition = Mathf.Clamp(newYPosition, transform.position.y, maxYValue);

                // Apply the new position to the object
                transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
            }
        }

        // Additionally, check if the plane's Y position exceeds the maximum limit and correct it if necessary
        if (transform.position.y > maxYValue)
        {
            // Clamp the Y position to the maximum allowed value
            transform.position = new Vector3(transform.position.x, maxYValue, transform.position.z);
        }
    }
}
