using UnityEngine;

public class ForwardMovement : MonoBehaviour
{
    public float speed = 5.0f; // Adjust the speed as needed

    void Update()
    {
        // Calculate the movement vector based on speed and time
        Vector3 movement = transform.forward * speed * Time.deltaTime;

        // Apply the movement to the object's position
        transform.position += movement;
    }
}