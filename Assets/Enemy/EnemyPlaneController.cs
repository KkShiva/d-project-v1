using System.Collections;
using UnityEngine;

public class EnemyPlaneController : MonoBehaviour
{
    public string targetTag = "Player"; // Tag of the object to follow
    public float followSpeed = 5f; // Speed of the enemy plane
    public GameObject prefabToSpawn; // Prefab to spawn
    public float minSpawnTime = 1f; // Minimum time interval for spawning
    public float maxSpawnTime = 5f; // Maximum time interval for spawning
    public Transform spawnPoint; // Spawn location (optional)

    private Transform target; // Reference to the target's transform
    public float interval = 30f; // Interval in seconds to update position


    void Start()
    {
        // Find the target by tag
        GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObject != null)
        {
            target = targetObject.transform;
            StartCoroutine(UpdatePosition());
        }
        else
        {
            Debug.LogError($"Target object with tag '{targetTag}' not found! Please set the correct tag.");
        }

        // Start the spawning coroutine
        StartCoroutine(SpawnPrefab());
    }


        // Coroutine to update position every interval
    private IEnumerator UpdatePosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval); // Wait for 30 seconds
            if (target != null)
            {
                transform.position = target.position; // Set position to target's position
            }
        }
    }

    void Update()
    {
        if (target != null)
        {
            // Move towards the target
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * followSpeed * Time.deltaTime;

            // Optionally rotate the enemy plane to face the target
           // Quaternion lookRotation = Quaternion.LookRotation(direction);
           // transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * followSpeed);
        }
    }

    IEnumerator SpawnPrefab()
    {
        while (true)
        {
            // Wait for a random time interval between minSpawnTime and maxSpawnTime
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            // Spawn the prefab
            if (prefabToSpawn != null)
            {
                Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
                Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Prefab to spawn is not assigned!");
            }
        }
    }
}
