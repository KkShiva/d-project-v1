using UnityEngine;

public class Magnate : MonoBehaviour
{
    public float range = 10f;  // Range within which the object will move toward the player
    public float moveSpeed = 5f; // Speed of the movement

    private Transform player;

    void Start()
    {
        // Find the player object by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No object with tag 'Player' found in the scene.");
        }
    }

    void Update()
    {
        if (player == null)
            return;

        // Calculate the distance between the object and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the player is within the specified range
        if (distanceToPlayer <= range && PlanePilot.instance.Magnet)
        {
            // Move towards the player
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    // Optional: Visualize the range in the scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
