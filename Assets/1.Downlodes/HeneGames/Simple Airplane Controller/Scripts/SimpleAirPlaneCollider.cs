using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene management functions

namespace HeneGames.Airplane
{
    public class SimpleAirPlaneCollider : MonoBehaviour
    {
        public bool collideSometing;

        public SimpleAirPlaneController controller;
         public string restartLayerName = "Blocks";
         
     public string nextSceneLayerName = "Next level"; // Name of the layer to move to the next scene

        private void OnTriggerEnter(Collider other)
        {
            //Collide someting bad
            if(other.gameObject.GetComponent<SimpleAirPlaneCollider>() == null && other.gameObject.GetComponent<LandingArea>() == null &&
            other.gameObject.layer == LayerMask.NameToLayer(restartLayerName))
            {
                collideSometing = true;
                // RestartScene();
            }
                        //Collide someting bad
            if(other.gameObject.GetComponent<SimpleAirPlaneCollider>() == null && other.gameObject.GetComponent<LandingArea>() == null &&
            other.gameObject.layer == LayerMask.NameToLayer(nextSceneLayerName))
            {
                collideSometing = true;
                 LoadNextScene();
            }
            
        }
    void RestartScene()
    {
        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();
        
        // Reload the current scene
        SceneManager.LoadScene(currentScene.buildIndex);
    }
        // Method to load the next scene
    public void LoadNextScene()
    {
        // Get the index of the current active scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Calculate the index for the next scene
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is within the valid range
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Load the next scene
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("This is the last scene, no more scenes to load.");
        }
    }
    }
}