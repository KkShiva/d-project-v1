using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene management functions
using UnityEngine.UI;
using TouchControlsKit;
using TMPro; // Required for TextMesh Pro
public class PlanePilot : MonoBehaviour
{
    public static PlanePilot instance;
    // Adjustable parameters
    [Header("Movement Settings")]
    public float minSpeed = 5f;         // Minimum speed
    public float maxSpeed = 20f;        // Maximum speed
    public float acceleration = 5f;     // Acceleration rate
    public float deceleration = 10f;    // Deceleration rate
    public float tiltAngle = 30f;       // Maximum tilt angle when turning

    public float gyroSensitivity = 2f; // Adjust to control the responsiveness of the gyroscope
    public float gyroSmoothness = 0.1f; // Lower values make input smoother but slower to respond
    private float smoothedGyroInput = 0f; // Used to store smoothed gyroscope input


    [Header("Joystick Input")]
    public string joystickAxis = "Joystick";
    public EAxisType axisType = EAxisType.Horizontal;

    private float currentSpeed;         // Current speed of the plane
    private float speedMultiplier = 1f; // Multiplier for speed, default is 1x
    private Coroutine activeSpeedCoroutine; // Tracks the active speed multiplier coroutine
    private float targetSpeed;          // Target speed of the plane

    
    [Header("VFX  Input")]
    public GameObject TwoxVfx;
    public GameObject TwoxVfxGui;
    public GameObject threexVfx;
    public GameObject threexVfxGui;
    
    public GameObject MagnetVfxGui;
    public GameObject MagnetVfx;
    public bool Magnet=false;
    [SerializeField] private TextMeshPro speedText; // Reference to the TextMeshPro object
 public bool collideSometing;


  public string Blocks = "Blocks";
  
    public float Blocks_Damage = 100f;
  public string Missile_1 = "Missile_1";
  
    public float Missile_1_Damage = 20f;
         
     public string nextSceneLayerName = "Next level"; // Name of the layer to move to the next scene
     
	public ParticleSystem Explotion;
        [Header("Health Settings")]
    public float maxHealth = 100f;
    public GameObject InvincibleVfx;
    public GameObject InvincibleVfxGui;

    private bool isInvincible = false;
    public float currentHealth;
    public Image healthBar;
    

        [Header("UI Components")]
    public TextMeshPro healthText;

     [Header("Score Settings")]
     public int score = 0;
     private int scoreMultiplier = 1; // Base multiplier (1x)
    private Coroutine activeMultiplierCoroutine; // To keep track of the active multiplier coroutine
    public TextMeshProUGUI scoreText; // Reference to your UI Text component

    [Header("Slider Settings")]
    [SerializeField] private Image PowerBar; // Reference to the Image with fillAmount
    [SerializeField] private float fillSpeed = 1f; // Speed at which the bar fills
    [SerializeField] private float maxFillTime = 3f; // Time to fully fill the bar
    private float fillTimer = 0f; // Tracks elapsed time
    private bool isFilling = false; // Controls whether the bar is filling

    [Header("Spawn Settings")]
    public GameObject objectToSpawn; // The GameObject to spawn
    public Transform spawnPosition; // Position where the object will be spawned

    [Header("Scene Settings")]
    public string firstSceneName = "1.Night";  // Replace with your first scene's name
    public string secondSceneName = "3.Red"; // Replace with your second scene's name


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }
    private void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
        // Initialize speed
        currentSpeed = minSpeed;
        TwoxVfx.SetActive(false);
        threexVfx.SetActive(false);
        InvincibleVfx.SetActive(false);
        MagnetVfx.SetActive(false);

        TwoxVfxGui.SetActive(false);
        threexVfxGui.SetActive(false);
        InvincibleVfxGui.SetActive(false);
        MagnetVfxGui.SetActive(false);

                // Enable the gyroscope if it is supported
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
        else
        {
            Debug.LogWarning("Gyroscope not supported on this device.");
        }
        StartFilling();

    }
    private void UpdateUI()
    {
        float fillValue = Mathf.Clamp01(currentHealth / maxHealth);
        // Update health text and slider
        if (healthText != null)
            healthText.text = $"{currentHealth:F1}";
            healthBar.fillAmount = fillValue;
        
        scoreText.text = "" + score;

    }

    public void TakeDamage(float damage)
    {
        // Reduce health and check for death
        currentHealth -= damage;
        UpdateUI();

        if (currentHealth <= 0)
        {
             // Load the first scene
            SceneManager.LoadScene("main menu");

        }
    }
    public void StartFilling()
    {
        isFilling = true;
    }

    public void StopFilling()
    {
        isFilling = false;
    }
    void SpawnTele()
    {
        if (objectToSpawn != null && spawnPosition != null)
        {
            Instantiate(objectToSpawn, spawnPosition.position, spawnPosition.rotation);
        }
        else
        {
            Debug.LogWarning("Object to spawn or spawn position is not set.");
        }
    }


    private void Update()
    {

        if (isFilling)
        {
            // Increment the fill timer
            fillTimer += Time.deltaTime * fillSpeed;

            // Update the PowerBar fillAmount (value between 0 and 1)
            PowerBar.fillAmount = fillTimer / maxFillTime;

            // Check if the bar is completely filled
            if (fillTimer >= maxFillTime)
            {
                // Spawn the object
                SpawnTele();

                // Reset the fill timer and PowerBar
                fillTimer = 0f;
                PowerBar.fillAmount = 0f;
            }
        }
        
        UpdateUI();
        // Update the TextMesh Pro text with the currentSpeed value
        speedText.text =  (currentSpeed * speedMultiplier).ToString("F2");
        // Get joystick input

       float inputH;

        //// Check if the gyroscope is enabled and use its data
        //if (Input.gyro.enabled)
        //{
        //    // Use the device's gyroscope rotation around the Y-axis
        //    // Use the device's gyroscope rotation around the Y-axis
        //    inputH = Input.gyro.rotationRateUnbiased.y * gyroSensitivity;
        //}
        //else
        //{
        //    // Fallback to joystick input (if gyroscope is unavailable or disabled)
        //    inputH = TCKInput.GetAxis(joystickAxis, axisType);
        //}

         inputH = TCKInput.GetAxis(joystickAxis, axisType);

        // Adjust target speed based on input
        targetSpeed = Mathf.Clamp(currentSpeed + inputH * acceleration * Time.deltaTime, minSpeed, maxSpeed);

        // Smoothly interpolate to the target speed
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, deceleration * Time.deltaTime);

        // Move the plane
        transform.Translate(Vector3.forward * currentSpeed * speedMultiplier * Time.deltaTime);

        // Apply horizontal sliding
        float slideAmount = inputH * currentSpeed * speedMultiplier * Time.deltaTime;
        transform.Translate(Vector3.right * slideAmount);

        // Tilt the plane based on input
        float tilt = Mathf.Lerp(0, -inputH * tiltAngle, Mathf.Abs(inputH));
        transform.rotation = Quaternion.Euler(0, 0, tilt);
    }
//

  private void OnTriggerEnter(Collider other)
     {


        if (other.gameObject.CompareTag("Invincible") && !isInvincible)
        {
            StartCoroutine(ActivateInvincibility());
            
        UpdateUI();
        }

        if (other.gameObject.CompareTag("Magnet") && !Magnet)
        {
            StartCoroutine(ActivateMagnet());
            
        UpdateUI();
        }

                // Check if the colliding object has the tag "TelepoteWorld"
        if (other.gameObject.CompareTag("TelepoteWorld"))
        {
            // Switch between the two scenes
            CheckAndLoadScene();
        }

        if(other.gameObject.layer == LayerMask.NameToLayer(Blocks))
         {
             collideSometing = true;
             TakeDamage(Blocks_Damage);
			Explotion.Play();
              //RestartScene();
              
		Explotion.Play();
         }
         //Collide someting bad
         if(other.gameObject.layer == LayerMask.NameToLayer(Missile_1))
         {
             collideSometing = true;
             TakeDamage(Missile_1_Damage);
			Explotion.Play();
              //RestartScene();
              
		Explotion.Play();
         }
                     //Collide someting bad
         if(other.gameObject.layer == LayerMask.NameToLayer(nextSceneLayerName))
         {
             collideSometing = true;
              LoadNextScene();
         }
        if (other.gameObject.CompareTag("coin"))
        {
            score += scoreMultiplier;
            scoreText.text = "Score: " + score;
            Destroy(other.gameObject); // Optional: Destroy the coin object
        }
        else if (other.gameObject.CompareTag("coin2"))
        {
            ActivateMultiplier(2, 30); // Double the score for 30 seconds
            Destroy(other.gameObject); // Optional: Destroy the coin2 object
        }
        else if (other.gameObject.CompareTag("coin3"))
        {
            ActivateMultiplier(3, 30); // Triple the score for 30 seconds
            Destroy(other.gameObject); // Optional: Destroy the coin4 object
        }


        if (other.gameObject.CompareTag("Speeddouble"))
        {
            ActivateSpeedMultiplier(2f, 10f); // Double the speed for 30 seconds
             ActivateMultiplier(2, 10); // Triple the score for 30 seconds
            Destroy(other.gameObject); // Optional: Destroy the power-up object
        }
        else if (other.gameObject.CompareTag("Speedtriple"))
        {
            
            ActivateSpeedMultiplier(3f, 10f); // Triple the speed for 30 seconds
            
             ActivateMultiplier(3, 10); // Triple the score for 30 seconds
            Destroy(other.gameObject); // Optional: Destroy the power-up object
        }
        else if (other.gameObject.CompareTag("Speedquadruple"))
        {
            ActivateSpeedMultiplier(4f, 10f); // Quadruple the speed for 30 seconds
            
             ActivateMultiplier(4, 10); // Triple the score for 30 seconds
            Destroy(other.gameObject); // Optional: Destroy the power-up object
        }
         
     }

     private void ActivateMultiplier(int multiplier, float duration)
    {
        if (activeMultiplierCoroutine != null)
        {
            // Stop the current multiplier coroutine to reset the duration
            StopCoroutine(activeMultiplierCoroutine);
        }

        // Start a new multiplier coroutine
        activeMultiplierCoroutine = StartCoroutine(MultiplierCoroutine(multiplier, duration));
    }

        private IEnumerator MultiplierCoroutine(int multiplier, float duration)
    {
        scoreMultiplier = multiplier; // Set the new multiplier
        yield return new WaitForSeconds(duration); // Wait for the specified duration
        scoreMultiplier = 1; // Reset the multiplier to the base value
        activeMultiplierCoroutine = null; // Clear the reference to the coroutine
    }



    private void ActivateSpeedMultiplier(float multiplier, float duration)
    {
        if (activeSpeedCoroutine != null)
        {
            // Stop the current speed multiplier coroutine to reset the duration
            StopCoroutine(activeSpeedCoroutine);
        }

        // Start a new speed multiplier coroutine
        activeSpeedCoroutine = StartCoroutine(SpeedMultiplierCoroutine(multiplier, duration));
        
                    
    }

    private IEnumerator SpeedMultiplierCoroutine(float multiplier, float duration)
    {
        speedMultiplier = multiplier; // Set the new multiplier
        if(multiplier==2)
        {
        TwoxVfx.SetActive(true);
        TwoxVfxGui.SetActive(true);
        }
        else if(multiplier==3)
        {
         threexVfx.SetActive(true);
         TwoxVfx.SetActive(true);
         threexVfxGui.SetActive(true);
         TwoxVfxGui.SetActive(true);
         }
        
        
        yield return new WaitForSeconds(duration); // Wait for the specified duration

        speedMultiplier = 1f; // Reset the multiplier to default
        
        TwoxVfx.SetActive(false);
        threexVfx.SetActive(false);
        TwoxVfxGui.SetActive(false);
        threexVfxGui.SetActive(false);

        activeSpeedCoroutine = null; // Clear the reference to the coroutine
    }

    private IEnumerator ActivateInvincibility()
    {
        
        isInvincible = true;
        float originalHealth = currentHealth; // Store the current health
        currentHealth = 9999999; // Set health to a very high value
        InvincibleVfx.SetActive(true);
        InvincibleVfxGui.SetActive(true);
        
        yield return new WaitForSeconds(20); // Wait for 30 seconds
        
        currentHealth = Mathf.Min(originalHealth, maxHealth); // Restore health, but not exceeding maxHealth
        isInvincible = false;
        InvincibleVfx.SetActive(false);
        InvincibleVfxGui.SetActive(false);
        
    }
    private IEnumerator ActivateMagnet()
    {
        
        Magnet = true;
        MagnetVfx.SetActive(true);
        MagnetVfxGui.SetActive(true);
        
        yield return new WaitForSeconds(20); // Wait for 30 seconds
        
        Magnet = false;
        MagnetVfx.SetActive(false);
        MagnetVfxGui.SetActive(false);
        
    }
        
   // private void OnCollisionEnter(Collision collision)
   // {
//
   //                 //Collide someting bad
   //         if(collision.gameObject.layer == LayerMask.NameToLayer(Missile_1))
   //         {
   //             collideSometing = true;
   //             TakeDamage(Missile_1_Damage);
	//		Explotion.Play();
   //              //RestartScene();
   //              
   //         }
   //                     //Collide someting bad
   //         if(collision.gameObject.layer == LayerMask.NameToLayer(nextSceneLayerName))
   //         {
   //             collideSometing = true;
   //              LoadNextScene();
   //         }
//
//
   // }
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

public void CheckAndLoadScene()
    {
        // Get the current active scene name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Check if the current scene is not one of the two specified scenes
        if (currentSceneName != firstSceneName && currentSceneName != secondSceneName)
        {
            // Load the first scene
            SceneManager.LoadScene(firstSceneName);
        }
        else
        {
            // Toggle between the first and second scenes
            if (currentSceneName == firstSceneName)
            {
                SceneManager.LoadScene(secondSceneName);
            }
            else if (currentSceneName == secondSceneName)
            {
                SceneManager.LoadScene(firstSceneName);
            }
        }
    }
    
}
