using System.Collections;
using UnityEngine;

public class SpawnPrefabsOnTagInRange : MonoBehaviour
{
    [SerializeField] private string targetTag = "Player"; // Tag of the object to detect
    [SerializeField] private float detectionRange = 5f; // Range within which the object is detected
    [SerializeField] private GameObject[] prefabsToSpawn; // Array of prefabs to spawn
    [SerializeField] private float spawnInterval = 1f; // Interval between spawns
    [SerializeField] private Transform[] spawnPoints; // Array of transforms to specify spawn positions

    private bool isSpawning = false;

    private void Update()
    {
        // Find all objects with the given tag
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        
        foreach (GameObject target in targets)
        {
            // Check distance to each target
            if (Vector3.Distance(transform.position, target.transform.position) <= detectionRange)
            {
                if (!isSpawning)
                {
                    StartCoroutine(SpawnPrefabs());
                }
                return; // Exit after finding the first valid target within range
            }
        }
        
        isSpawning = false; // Stop spawning if no target is in range
    }

    private IEnumerator SpawnPrefabs()
    {
        isSpawning = true;

        for (int i = 0; i < 5; i++) // Spawn prefabs 5 times
        {
            for (int j = 0; j < spawnPoints.Length; j++)
            {
                if (j < prefabsToSpawn.Length) // Ensure matching index
                {
                    Instantiate(prefabsToSpawn[j], spawnPoints[j].position, spawnPoints[j].rotation);
                }
            }
            yield return new WaitForSeconds(spawnInterval); // Wait for the specified interval
        }

        isSpawning = false; // Reset the flag after spawning
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the detection range in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Visualize the spawn points in the editor
        Gizmos.color = Color.red;
        if (spawnPoints != null)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                Gizmos.DrawSphere(spawnPoint.position, 0.2f);
            }
        }
    }
}
