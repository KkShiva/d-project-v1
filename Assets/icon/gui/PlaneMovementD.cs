using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;  // Required for Pointer events
using TMPro;

public class PlaneMovementD : MonoBehaviour
{
    public float moveUpSpeed = 5f;  // Speed for moving up
    public float slideSpeed = 5f;   // Speed for sliding left and right
    public GameObject leftButton;   // Reference to the Left Button UI (GameObject)
    public GameObject rightButton;  // Reference to the Right Button UI (GameObject)
    public GameObject particlePrefab; // Prefab for the particle effect
    private bool moveLeft = false;  // State for moving left
    private bool moveRight = false; // State for moving right
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public LayerMask wallLayer; // Layer for Wall objects
    public GameObject tail1;  
    public GameObject tail2;  
    public GameObject tail3;  
    public GameObject tail4;  
    public TextMeshProUGUI textMeshPro;  // Reference to TextMeshPro UI Text
    private Transform objTransform;      // Reference to the object's Transform

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get the Rigidbody2D component
        spriteRenderer = GetComponent<SpriteRenderer>();  // Get the SpriteRenderer component

        // Add event listeners for holding down and releasing the buttons
        AddButtonEvents(leftButton, OnLeftButtonDown, OnButtonUp);
        AddButtonEvents(rightButton, OnRightButtonDown, OnButtonUp);
        GetComponent<Rigidbody2D>().isKinematic = true;
         // Get the Transform component of the GameObject this script is attached to
        objTransform = GetComponent<Transform>();
    }

    private void Update()
    {
       Invoke("MoveUp", 4.0f);
        Slide();   // Slide the plane based on button press and hold
        // Get the Y position of the object
        float yPos = objTransform.position.y;

        // Assign the Y position to the TextMeshPro text, converted to a string
        textMeshPro.text = " Distance : " + yPos.ToString("F0");  // Optional: Format to 2 decimal places
    }

    private void MoveUp()
    {
        GetComponent<Rigidbody2D>().isKinematic = false;
        // Constant upward movement on the Y-axis
        rb.velocity = new Vector2(rb.velocity.x, moveUpSpeed);
    }

    private void Slide()
    {
        float slideDirection = 0f;

        if (moveLeft)
        {
            slideDirection = -1f;  // Slide left
        }
        else if (moveRight)
        {
            slideDirection = 1f;   // Slide right
        }

        // Apply horizontal movement to the plane
        rb.velocity = new Vector2(slideDirection * slideSpeed, rb.velocity.y);
    }

    // Called when the left button is held down
    public void OnLeftButtonDown()
    {
        moveLeft = true;
        moveRight = false;
    }

    // Called when the right button is held down
    public void OnRightButtonDown()
    {
        moveRight = true;
        moveLeft = false;
    }

    // Called when a button is released (stop sliding)
    public void OnButtonUp()
    {
        moveLeft = false;
        moveRight = false;
    }

    // Helper function to add PointerDown and PointerUp events
    private void AddButtonEvents(GameObject button, UnityEngine.Events.UnityAction onDown, UnityEngine.Events.UnityAction onUp)
    {
        EventTrigger trigger = button.GetComponent<EventTrigger>();

        if (trigger == null)
        {
            trigger = button.AddComponent<EventTrigger>();
        }

        // Add PointerDown event (for press and hold)
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((data) => { onDown(); });
        trigger.triggers.Add(pointerDownEntry);

        // Add PointerUp event (for release)
        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((data) => { onUp(); });
        trigger.triggers.Add(pointerUpEntry);
    }

    // This function detects collision with "Wall" objects
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object belongs to the Wall layer
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            // Disable sprite rendering (simulate disabling sprint)
            spriteRenderer.enabled = false;
            tail1.SetActive(false);
            tail2.SetActive(false);
            tail3.SetActive(false);
            tail4.SetActive(false);

            // Instantiate particle effect at the plane's position
            Instantiate(particlePrefab, transform.position, Quaternion.identity);
           
            Invoke("OnReset", 2.0f);
            // Constant upward movement on the Y-axis
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }
    public void OnReset()
    {
        transform.position = new Vector3(0, 0, 600);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        spriteRenderer.enabled = true;
        
        tail1.SetActive(true);
        tail2.SetActive(true);
        tail3.SetActive(true);
        tail4.SetActive(true);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.freezeRotation = true;
        MoveUp();
    }

}
