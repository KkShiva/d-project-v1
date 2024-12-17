using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;           // The prefab to spawn
    public int numberOfPrefabs = 5;            // The number of prefabs to spawn
    public List<GameObject> spawnPoints;       // List of spawn point GameObjects
    public float timeInterval = 2f;            // Time interval between spawns

    private void Start()
    {
        // Start the coroutine to spawn prefabs
        StartCoroutine(SpawnPrefabs());
    }

    private IEnumerator SpawnPrefabs()
    {
        for (int i = 0; i < 1000000; i++)
        {
            // Wait for the specified time interval between each spawn
            yield return new WaitForSeconds(timeInterval);

            // Select a random spawn point from the list of spawnPoints
            int randomIndex = Random.Range(0, spawnPoints.Count);
            GameObject randomSpawnPoint = spawnPoints[randomIndex];

            // Instantiate the prefab at the random spawn point's position
            Instantiate(prefabToSpawn, randomSpawnPoint.transform.position, Quaternion.identity);
        }
    }
}
