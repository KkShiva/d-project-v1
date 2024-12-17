using UnityEngine;

public class CustomLowGravityFall : MonoBehaviour
{
   // public float gravityScale = 0.2f; // Adjust the gravity strength
//
   // private Vector3 velocity;
    public LayerMask targetLayer; // Assign the layer to check for collision
    private Rigidbody rb;


    void Update()
    {
       // // Apply gravity
       // velocity.y -= gravityScale * Time.deltaTime;
//
       // // Apply velocity to the object's position
       // transform.position += velocity * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is on the target layer
        if (collision.gameObject.layer == targetLayer)
        {
            // Disable this script
            this.enabled = false;
            rb.isKinematic = true; // Disable physics interactions
        }
    }
}