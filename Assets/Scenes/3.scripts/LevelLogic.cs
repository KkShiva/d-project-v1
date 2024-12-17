using UnityEngine;
using UnityEngine.SceneManagement; // For scene management functions
using TMPro;
using TouchControlsKit;

public class LevelLogic : MonoBehaviour
{
    public Transform object1; // First object
    public Transform object2; // Second object
    public TextMeshProUGUI distanceText; // TextMeshPro component to display the distance
    //public string restartLayerName = "RestartLayer"; // Name of the layer to restart the scene on collision
    //public string nextSceneLayerName = "NextSceneLayer"; // Name of the layer to move to the next scene
   // public int sceneToLoad ; // Scene number to load on collision with 'NextSceneLayer'
    public int level=1;
    public bool islevel = true;
    public GameObject TPSCam;
    public GameObject FPSCam;
    private bool IsTPS;
    void Start()
    {
        TPSCam.SetActive(true);
        FPSCam.SetActive(false);
        IsTPS=true;

        if(islevel = true)
        {
        SaveLevel(level);
        }
    }
    void Update()
    {
        if(TCKInput.GetAction( "camara", EActionEvent.Click) && IsTPS)
        {
        TPSCam.SetActive(false);
        FPSCam.SetActive(true);
        IsTPS=false;
        }else if(TCKInput.GetAction( "camara", EActionEvent.Click) && !IsTPS)
        {
        TPSCam.SetActive(true);
        FPSCam.SetActive(false);  
        IsTPS=true;
        }
        // Calculate the distance between object1 and object2
        float distance = Vector3.Distance(object1.position, object2.position);

        // Update the TextMeshPro text to display the distance
        distanceText.text =  distance.ToString("F0") + " units";

        if(distance > 600.00)
        {
             RestartScene();
        }
    }
        // Function to save the current level
    public void SaveLevel(int level)
    {
        // Save the level using PlayerPrefs
        PlayerPrefs.SetInt("SavedLevel", level);
        PlayerPrefs.Save();
        Debug.Log("Level " + level + " saved.");
    }
    void OnCollisionEnter(Collision collision)
    {
        // Check if the object we collided with is on the restart layer
        //if (collision.gameObject.layer == LayerMask.NameToLayer(restartLayerName))
        //{
            // Restart the current scene
       //     RestartScene();
       // }
        // Check if the object we collided with is on the next scene layer
       // if (collision.gameObject.layer == LayerMask.NameToLayer(nextSceneLayerName))
       // {
       //     // Load the specified scene
       //     LoadScene(sceneToLoad);
       // }
    }

    // Method to restart the current scene
    void RestartScene()
    {
        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();
        
        // Reload the current scene
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    // Method to load a specific scene by its build index
    void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
