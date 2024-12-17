using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomChildHandler : MonoBehaviour
{
    public GameObject parentObject; // The parent object whose children you want to enable/disable
    public float totalTime = 5f; // Total time to enable/disable all children
    public bool performEnable = false; // Public bool to determine whether to enable or disable children

    void Start()
    {
        if (performEnable)
        {
            // Start coroutine to enable random children
            StartCoroutine(EnableRandomChildren());
        }
        else
        {
            // Start coroutine to disable random children
            StartCoroutine(DisableRandomChildren());
        }
    }

    IEnumerator DisableRandomChildren()
    {
        // Get a list of all child transforms
        List<Transform> children = new List<Transform>();

        foreach (Transform child in parentObject.transform)
        {
            children.Add(child);
        }

        // Shuffle the list of children randomly
        ShuffleList(children);

        // Calculate the delay between disabling each child
        float delayBetweenDisables = totalTime / children.Count;

        // Loop through the shuffled children list
        foreach (Transform child in children)
        {
            // Disable the child
            child.gameObject.SetActive(false);

            // Wait for the calculated delay before disabling the next random child
            yield return new WaitForSeconds(delayBetweenDisables);
        }
    }

    IEnumerator EnableRandomChildren()
    {
        // Get a list of all child transforms
        List<Transform> children = new List<Transform>();

        foreach (Transform child in parentObject.transform)
        {
            children.Add(child);
        }

        // Shuffle the list of children randomly
        ShuffleList(children);

        // Calculate the delay between enabling each child
        float delayBetweenEnables = totalTime / children.Count;

        // Loop through the shuffled children list
        foreach (Transform child in children)
        {
            // Enable the child
            child.gameObject.SetActive(true);

            // Wait for the calculated delay before enabling the next random child
            yield return new WaitForSeconds(delayBetweenEnables);
        }
    }

    // Function to shuffle the list of child transforms
    void ShuffleList(List<Transform> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Transform temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
