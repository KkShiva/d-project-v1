using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

    public string sceneToDestroy = "main menu"; // Name of the scene where this object should be destroyed
    public Transform childObject ; // Name of the child object to reset position
    public Transform E1Object ; // Name of the child object to reset position
    public Transform E2Object ; // Name of the child object to reset position
    public Transform E3Object ; // Name of the child object to reset position

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
            return;
        }

        // Reset position of the specified child object
        ResetChildObjectPosition();
    }

    void ResetChildObjectPosition()
    {
        // Find the child object by name
        //Transform childObject = transform.Find(childObjectName);
      
            childObject.position = new Vector3(0, 30, 0);

            E1Object.position = new Vector3(0,1,-70);
            E2Object.position = new Vector3(0,1,-70);
            E3Object.position = new Vector3(0,1,-70);

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
