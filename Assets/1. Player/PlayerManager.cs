using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

    public string sceneToDestroy = "main menu"; // Name of the scene where this object should be destroyed

    void Awake()
    {
        // Check if this is the first instance of GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManagers
            return;
        }

        // Check if the current scene is the one to destroy GameManager
        if (SceneManager.GetActiveScene().name == sceneToDestroy)
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Destroy GameManager when entering the specified scene
        if (scene.name == sceneToDestroy)
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from the sceneLoaded event
    }
}
