using UnityEngine;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI highestTimeText;

    private float timeElapsed;
    private float highestTime;

    private void Start()
    {
        // Load the highest time from PlayerPrefs if available
        highestTime = PlayerPrefs.GetFloat("HighestTime", 0);
        UpdateHighestTimeText();
    }

    private void Update()
    {
        // Update the elapsed time
        timeElapsed += Time.deltaTime;
        UpdateCurrentTimeText();

        // Check if the current time is greater than the highest time
        if (timeElapsed > highestTime)
        {
            highestTime = timeElapsed;
            UpdateHighestTimeText();
            SaveHighestTime();
        }
    }

    private void UpdateCurrentTimeText()
    {
        // Format the current time as hh:mm:ss
        int hours = Mathf.FloorToInt(timeElapsed / 3600);
        int minutes = Mathf.FloorToInt((timeElapsed % 3600) / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        currentTimeText.text = string.Format("Current Timer : {0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    private void UpdateHighestTimeText()
    {
        // Format the highest time as hh:mm:ss
        int hours = Mathf.FloorToInt(highestTime / 3600);
        int minutes = Mathf.FloorToInt((highestTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(highestTime % 60);
        highestTimeText.text = string.Format("Last best Time: {0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    private void SaveHighestTime()
    {
        // Save the highest time to PlayerPrefs
        PlayerPrefs.SetFloat("HighestTime", highestTime);
        PlayerPrefs.Save();
    }
}
