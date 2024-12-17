using System.Collections;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float normalRotationSpeed = 50f;  // Speed of the normal rotation
    public bool enableGlitch = false;        // Toggle for glitch effect
    public float glitchIntensity = 30f;      // How much the object should glitch (rotation variance)
    public float glitchDuration = 0.1f;      // Duration of each glitch in seconds

    private bool isGlitching = false;

    void Update()
    {
        // Normal rotation
        if (!isGlitching)
        {
            RotateNormally();
        }

        // Check if glitch effect is enabled
        if (enableGlitch && !isGlitching)
        {
            StartCoroutine(GlitchRotation());
        }
    }

    void RotateNormally()
    {
        // Rotate the object around its Y axis at the defined speed
        transform.Rotate(Vector3.up * normalRotationSpeed * Time.deltaTime);
    }

    IEnumerator GlitchRotation()
    {
        isGlitching = true;

        // Randomize the rotation during the glitch
        Vector3 randomRotation = new Vector3(
            Random.Range(-glitchIntensity, glitchIntensity),
            Random.Range(-glitchIntensity, glitchIntensity),
            Random.Range(-glitchIntensity, glitchIntensity)
        );

        // Apply the glitchy rotation
        transform.Rotate(randomRotation);

        // Wait for the glitch duration
        yield return new WaitForSeconds(glitchDuration);

        // End the glitch effect
        isGlitching = false;
    }
}
