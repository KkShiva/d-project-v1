using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    public float delayTime = 5f;  // Time to wait before transitioning to the next scene

    void Start()
    {
        // Start the coroutine to handle the delay and scene change
        StartCoroutine(GoToNextSceneAfterDelay());
    }

    IEnumerator GoToNextSceneAfterDelay()
    {
        // Wait for the specified time
        yield return new WaitForSeconds(delayTime);

        // Get the current scene's build index and add 1 to load the next scene
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Check if the next scene index is valid (i.e., within the range of scenes in the build)
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Load the next scene
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogError("No next scene in the build settings!");
        }
    }
}
