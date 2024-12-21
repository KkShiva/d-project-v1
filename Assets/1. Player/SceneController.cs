using UnityEngine;
using UnityEngine.SceneManagement; // For scene management functions

public class SceneController : MonoBehaviour
{
    private AudioSource[] allAudioSources;
    private bool isPaused = false;
    public GameObject PauseScreen;


    void Start()
    {
        // Get all audio sources in the scene
        allAudioSources = FindObjectsOfType<AudioSource>();
    }
    // Reload the current scene
    public void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    // Load a specific scene by its build index
    public void OpenSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
        public void mainmenu()
    {
        SceneManager.LoadScene(0);
        
        Time.timeScale = 1f; // Resumes the game’s time
        
    }

    // Restart the current scene (essentially the same as ReloadScene)
    public void RestartScene()
    {
        ReloadScene();
    }

    // Pause the game
    public void PauseGame()
    {
        PauseScreen.SetActive(true);
        Time.timeScale = 0f; // Stops the game’s time
        PauseAllAudio();
    }

    // Resume the game from a paused state
    public void ResumeGame()
    {
        PauseScreen.SetActive(false);
        Time.timeScale = 1f; // Resumes the game’s time
        ResumeAllAudio();
    }

    // Method to pause all audio
    public void PauseAllAudio()
    {
        if (!isPaused)
        {
            foreach (AudioSource audioSource in allAudioSources)
            {
                if (audioSource != null && audioSource.isPlaying) // Check if audioSource exists
                {
                    audioSource.Pause();
                }
            }
            isPaused = true;
        }
    }

    // Method to resume all audio
    public void ResumeAllAudio()
    {
        if (isPaused)
        {
            foreach (AudioSource audioSource in allAudioSources)
            {
                if (audioSource != null) // Check if audioSource exists
                {
                    audioSource.UnPause();
                }
            }
            isPaused = false;
        }
    }
     // Function to load the saved level
    public void ContinueGame()
    {
        // Check if there is a saved level
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            int savedLevel = PlayerPrefs.GetInt("SavedLevel");
            Debug.Log("Loading saved level: " + savedLevel);
            
            // Load the saved level (replace with your scene loading logic)
            SceneManager.LoadScene(savedLevel);
        }
        else
        {
            Debug.Log("No saved level found. Starting new game.");
            // Load the first level (e.g., level 1 or your main menu)
            SceneManager.LoadScene(1); // Change '1' to your actual first level index
        }
    }

    // Optional function to reset saved data
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("SavedLevel");
        Debug.Log("Progress reset.");
    }
}
