using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public GameObject pickupEffect;    
    public string targetLayerName;

    private int targetLayer;

    void Start()
    {
        // Get the layer index from the layer name.
        targetLayer = LayerMask.NameToLayer(targetLayerName);


        if (targetLayer == -1)
        {
            Debug.LogError($"Layer '{targetLayerName}' does not exist!");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is on the target layer.
        if (other.gameObject.layer == targetLayer)
        {
           
            Pickup();
            
        }
    }

    void Pickup()
    {
        Instantiate(pickupEffect, transform.position, transform.rotation);


        Destroy(gameObject);
    }
}
