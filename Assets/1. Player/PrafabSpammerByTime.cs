using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrafabSpammerByTime : MonoBehaviour
{

    public GameObject prefabToSpawn; // Prefab to spawn
    public float minSpawnTime = 1f; // Minimum time interval for spawning
    public float maxSpawnTime = 5f; // Maximum time interval for spawning
    public Transform spawnPoint; // Spawn location (optional)
    // Start is called before the first frame update
    void Start()
    {
        
            StartCoroutine(SpawnPrefab());
    }

    // Update is called once per frame
    void Update()
    {
        
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
