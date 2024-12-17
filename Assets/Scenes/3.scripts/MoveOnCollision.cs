using UnityEngine;
using System.Collections;

public class MoveOnCollision : MonoBehaviour
{
    public string targetLayer;      // The layer that triggers the collision
    public GameObject targetObject; // The object whose Y position will be changed
    public float yIncrease = 5f;    // How much to increase the Y position
    public float moveTime = 2f;     // How long to take to increase the Y

    private int targetLayerID;      // The layer ID

    void Start()
    {
        // Get the layer ID from the layer name
        targetLayerID = LayerMask.NameToLayer(targetLayer);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is on the specified layer
        if (other.gameObject.layer == targetLayerID)
        {
            // Start the movement coroutine
            StartCoroutine(MoveObject(targetObject.transform, yIncrease, moveTime));
        }
    }

    IEnumerator MoveObject(Transform objTransform, float yValue, float duration)
    {
        float elapsedTime = 0;
        Vector3 initialPosition = objTransform.position;
        Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y + yValue, initialPosition.z);

        while (elapsedTime < duration)
        {
            objTransform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Make sure the object ends at the exact target position
        objTransform.position = targetPosition;
    }
}