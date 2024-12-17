using UnityEngine;
using TouchControlsKit;

public class CameraControl : MonoBehaviour
{
    public float horizontalAimingSpeed = 2.0f; // Speed of horizontal camera movement
    public float verticalAimingSpeed = 2.0f;   // Speed of vertical camera movement
    public float minVerticalAngle = -60f;      // Minimum vertical angle limit
    public float maxVerticalAngle = 60f;       // Maximum vertical angle limit

    private float angleH = 0f; // Horizontal angle (yaw)
    private float angleV = 0f; // Vertical angle (pitch)

    void Update()
    {
        // Get input from the axes (Touchpad or any other input defined in InputManager)
        float horizontalInput = Mathf.Clamp(TCKInput.GetAxis("Touchpad", EAxisType.Horizontal), -1, 1);
        float verticalInput = Mathf.Clamp(TCKInput.GetAxis("Touchpad", EAxisType.Vertical), -1, 1);

        // Calculate the new angles based on input and aiming speed
        angleH += horizontalInput * horizontalAimingSpeed;
        angleV += verticalInput * verticalAimingSpeed;

        // Clamp the vertical angle to prevent over-rotation
        angleV = Mathf.Clamp(angleV, minVerticalAngle, maxVerticalAngle);

        // Apply the rotations to the camera
        transform.localRotation = Quaternion.Euler(-angleV, angleH, 0f);
    }
}
