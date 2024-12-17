using UnityEngine;
using System.Collections.Generic;

public class InfiniteGround : MonoBehaviour
{
    [Header("Plane Prefabs")]
    public GameObject[] planePrefabs; // Array of plane prefabs to instantiate

    [Header("Player and Grid Settings")]
    public Transform player; // The player object
    public Camera mainCamera; // The main camera
    public Vector2Int gridSize = new Vector2Int(3, 3); // Size of the grid (3x3)
    public int planeSize = 2000; // Size of each plane (assumed to be square)

    private Dictionary<Vector2Int, GameObject> activePlanes = new Dictionary<Vector2Int, GameObject>(); // Stores active planes
    private Vector2Int currentPlanePosition = Vector2Int.zero; // The current plane the player is on
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
        
        // Instantiate the initial grid of planes
        GeneratePlanes(currentPlanePosition);
    }

    void Update()
    {
        Vector2Int playerGridPosition = GetPlayerGridPosition();

        // If player moved to a new grid, generate new planes
        if (playerGridPosition != currentPlanePosition)
        {
            currentPlanePosition = playerGridPosition;
            GeneratePlanes(currentPlanePosition);
        }
    }

    // Calculate which grid the player is currently on
    Vector2Int GetPlayerGridPosition()
    {
        int x = Mathf.FloorToInt(player.position.x / planeSize);
        int z = Mathf.FloorToInt(player.position.z / planeSize);
        return new Vector2Int(x, z);
    }

    // Generate planes around the player's current position
    void GeneratePlanes(Vector2Int playerGridPosition)
    {
        // Destroy planes that are no longer adjacent, diagonal, or visible
        List<Vector2Int> keysToRemove = new List<Vector2Int>();
        foreach (var pos in activePlanes.Keys)
        {
            if (Vector2Int.Distance(playerGridPosition, pos) > 1.5f && !IsPlaneVisible(activePlanes[pos]))
            {
                keysToRemove.Add(pos);
            }
        }

        foreach (var pos in keysToRemove)
        {
            Destroy(activePlanes[pos]);
            activePlanes.Remove(pos);
        }

        // Instantiate planes at adjacent and diagonal positions
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                Vector2Int newPos = new Vector2Int(playerGridPosition.x + x, playerGridPosition.y + z);

                if (!activePlanes.ContainsKey(newPos))
                {
                    Vector3 position = new Vector3(newPos.x * planeSize, 0, newPos.y * planeSize);

                    // Randomly choose a plane prefab from the array
                    GameObject randomPrefab = GetRandomPrefab();
                    
                    // Instantiate the chosen plane prefab
                    GameObject newPlane = Instantiate(randomPrefab, position, Quaternion.identity);
                    activePlanes.Add(newPos, newPlane);
                }
            }
        }
    }

    // Helper function to get a random plane prefab from the array
    GameObject GetRandomPrefab()
    {
        if (planePrefabs.Length == 0) return null; // Safety check to ensure there are prefabs
        int randomIndex = Random.Range(0, planePrefabs.Length);
        return planePrefabs[randomIndex];
    }

// Check if a plane is visible by the camera
bool IsPlaneVisible(GameObject plane)
{
    if (mainCamera == null) return false;

    // Attempt to get the Renderer component
    Renderer planeRenderer = plane.GetComponent<Renderer>();
    if (planeRenderer == null)
    {
        Debug.LogWarning($"Renderer not found on {plane.name}. Plane visibility cannot be determined.");
        return false; // If no Renderer, consider it not visible
    }

    Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
    Bounds planeBounds = planeRenderer.bounds;
    return GeometryUtility.TestPlanesAABB(frustumPlanes, planeBounds);
}
}
