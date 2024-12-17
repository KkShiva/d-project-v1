using UnityEngine;

public class ScaleChanger : MonoBehaviour
{
    // Initial and target scale
    public float startScale = 0.2f;
    public float targetScale = 1.0f;

    // Time to scale
    public float duration = 2.0f;

    private float elapsedTime = 0.0f;
    private Vector3 initialScale;
    private Vector3 finalScale;

    void Start()
    {
        // Set the initial and target scales
        initialScale = Vector3.one * startScale;
        finalScale = Vector3.one * targetScale;

        // Initialize the object's scale
        transform.localScale = initialScale;
    }

    void Update()
    {
        // Increase elapsed time
        elapsedTime += Time.deltaTime;

        // Calculate the scale factor using Lerp
        float t = Mathf.Clamp01(elapsedTime / duration);
        transform.localScale = Vector3.Lerp(initialScale, finalScale, t);

        // Optional: Stop updating after reaching the target scale
        if (t >= 1.0f)
        {
            enabled = false;
        }
    }
}
