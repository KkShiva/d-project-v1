using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DableObjectOnCollision : MonoBehaviour
{
 [Tooltip("The object to enable when the trigger is activated.")]
    public GameObject objectToEnable;

    [Tooltip("The layer to check for collision.")]
    public string targetLayerName;

    private int targetLayer;

    void Start()
    {
        // Get the layer index from the layer name.
        targetLayer = LayerMask.NameToLayer(targetLayerName);

        if (objectToEnable == null)
        {
            Debug.LogWarning("No object is assigned to enable!");
        }
        if (targetLayer == -1)
        {
            Debug.LogError($"Layer '{targetLayerName}' does not exist!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is on the target layer.
        if (other.gameObject.layer == targetLayer)
        {
            if (objectToEnable != null)
            {
                objectToEnable.SetActive(false);
                Debug.Log($"{objectToEnable.name} disabled by collision with {other.gameObject.name}");
            }
        }
    }
}