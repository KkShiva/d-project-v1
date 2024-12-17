using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaneMovement2 : MonoBehaviour
{
    public float distanceFromGround = 20f; // Desired distance from the ground
    public LayerMask groundLayer;        // Layer mask for the ground


    void Start()
    {
    }

    void Update()
    {
        // Adjust the height to maintain the desired distance from the ground
        MaintainGroundDistance();

        // Ensure the x-axis rotation stays at 0
        KeepRotationFixed();
    }

    void MaintainGroundDistance()
    {
        RaycastHit hit;
        // Cast a ray downwards from the object's position
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            // Check the distance between the object and the ground
            float currentDistance = hit.distance;

            // Adjust the object's height to maintain the desired distance
            if (Mathf.Abs(currentDistance - distanceFromGround) > 0.1f)
            {
                float heightAdjustment = distanceFromGround - currentDistance;
                transform.position = new Vector3(transform.position.x, transform.position.y + heightAdjustment, transform.position.z);
            }
        }
    }

    void KeepRotationFixed()
    {
        // Lock the x-axis rotation at 0, keep other rotations as they are
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
    
}
