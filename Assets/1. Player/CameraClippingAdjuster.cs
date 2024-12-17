using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CameraClippingAdjuster : MonoBehaviour
{
    public Camera targetCamera;       // Reference to the camera
    public float farClipStart = 100f;  // Starting far clipping plane
    public float farClipEnd = 500f;    // Target far clipping plane (B)
    public float transitionDuration = 5f; // Time for the transition

    private float elapsedTime = 0f;   // Tracks time for the transition
    private bool isAdjusting = false; // Whether the adjustment is active
  public float delayBeforeSkyboxRemoval = 3f; // Time (in seconds) before disabling the skybox
    

        void Awake()
    {
        // Ensure the GameObject persists across scenes
        DontDestroyOnLoad(gameObject);

        // Register for the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Unregister from the sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
   void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update the camera reference for the new scene
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }        

        StartCoroutine(RemoveSkyboxAfterDelay());

        // Automatically start the adjustment when a new scene loads
        StartAdjustment();
    }

    void Update()
    {
        // If adjustment is active, interpolate the clipping planes
        if (isAdjusting)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            // Smoothly interpolate near and far clipping planes
            targetCamera.farClipPlane = Mathf.Lerp(farClipStart, farClipEnd, t);

            // Stop adjusting if the transition is complete
            if (t >= 1f)
            {
                isAdjusting = false;
            }
        }
    }

    public void StartAdjustment()
    {
        // Reset transition variables and start adjustment
        elapsedTime = 0f;
        isAdjusting = true;
    }
        private IEnumerator RemoveSkyboxAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayBeforeSkyboxRemoval);

        // Disable the global skybox
        RenderSettings.skybox = null;

        // Optionally, log the change for debugging
        Debug.Log("Skybox has been removed after delay.");
    }
}
