using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Prefab to Spawn")]
    public GameObject objectPrefab;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Spawn Settings")]
    public float spawnInterval = 1.0f; // Time interval between spawns
    public int minSpawnPoints = 30;    // Minimum number of points to spawn objects at
    public int maxSpawnPoints = 51;    // Maximum number of points to spawn objects at

    private float timer;

    void Start()
    {
        timer = spawnInterval; // Start with the initial spawn delay
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpamObjects();
            timer = spawnInterval; // Reset timer for the next spawn
        }
    }

    void SpamObjects()
    {
        // Determine how many objects to spawn in this interval (random between min and max)
        int objectsToSpawn = Random.Range(minSpawnPoints, maxSpawnPoints);

        // Shuffle the spawn points so we can randomly choose
        Transform[] shuffledSpawnPoints = ShuffleSpawnPoints();

        // Spawn objects at random spawn points
        for (int i = 0; i < objectsToSpawn; i++)
        {
            Transform spawnPoint = shuffledSpawnPoints[i]; // Get a shuffled spawn point
            // Instantiate the prefab at the spawn point position and rotation
            GameObject spawnedObject = Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    // Function to shuffle the spawn points
    Transform[] ShuffleSpawnPoints()
    {
        Transform[] shuffled = spawnPoints.Clone() as Transform[];

        for (int i = 0; i < shuffled.Length; i++)
        {
            int randomIndex = Random.Range(i, shuffled.Length);
            Transform temp = shuffled[i];
            shuffled[i] = shuffled[randomIndex];
            shuffled[randomIndex] = temp;
        }

        return shuffled;
    }
}
