using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusMove : MonoBehaviour
{
    private float cycle; // This variable increases with time and allows the sine to produce numbers between -1 and 1.
    private Vector3 basePosition; // This variable maintains the location of the object without applying sine changes

    public Transform target;

    public float waveSpeed = 1f; // Higher makes the wave faster
    public float verticalWaveHeight = 1f; // Set higher if you want more wave intensity in the vertical direction
    public float horizontalWaveWidth = 1f; // Set higher if you want more wave intensity in the horizontal direction
    public float speed = 1f; // More value moves the object faster to the target

    public LayerMask targetLayer; // Layer you want to maintain distance from
    public float distanceToLayer = 2f; // Desired distance from the layer

    public void Start() => basePosition = transform.position;

    void Update()
    {
        // Increment cycle based on time and waveSpeed
        cycle += Time.deltaTime * waveSpeed;

        // Calculate the new position based on sine waves in both vertical and horizontal axes
        Vector3 sineWavePosition = new Vector3(Mathf.Sin(cycle) * horizontalWaveWidth, Mathf.Sin(cycle) * verticalWaveHeight, 0f);

        // Update the object's position with both vertical and horizontal wave movement
        transform.position = basePosition + sineWavePosition;

        // Move towards the target, updating the base position
        if (target)
        {
            basePosition = Vector3.MoveTowards(basePosition, target.position, Time.deltaTime * speed);
        }

        // Maintain the desired distance from the layer
        MaintainDistanceFromLayer();
    }

    void MaintainDistanceFromLayer()
    {
        RaycastHit hit;

        // Cast a ray downward to check the distance from the layer
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, targetLayer))
        {
            float currentDistance = hit.distance;

            // Check if the object is too close or too far from the layer
            if (Mathf.Abs(currentDistance - distanceToLayer) > 0.01f)
            {
                // Calculate the direction to move the object to maintain the desired distance
                Vector3 direction = (transform.position - hit.point).normalized;

                // Adjust the position to maintain the desired distance
                transform.position = hit.point + direction * distanceToLayer;
                basePosition = transform.position; // Update basePosition to avoid jittering
            }
        }
    }
}
