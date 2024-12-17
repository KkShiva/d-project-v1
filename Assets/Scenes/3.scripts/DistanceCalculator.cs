using UnityEngine;
using TMPro;

public class DistanceCalculator : MonoBehaviour
{
    public Transform object1; // First object
    public Transform object2; // Second object
    public TextMeshProUGUI distanceText; // TextMeshPro component to display the distance

    void Update()
    {
        // Calculate the distance between object1 and object2
        float distance = Vector3.Distance(object1.position, object2.position);

        // Update the TextMeshPro text to display the distance
        distanceText.text =  distance.ToString("F0") + " units";
    }
}
