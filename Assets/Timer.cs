using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//learned from Tutorial: https://www.youtube.com/watch?v=POq1i8FyRyQ&t=12s 

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime = 0f;
    float intervalTimer = 0f;
    int hours = 12; // Start at 12
    int minutes = 0;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        intervalTimer += Time.deltaTime;

        // Check if 7 seconds have passed
        if (intervalTimer >= 7f)
        {
            minutes += 15;
            intervalTimer = 0f; // Reset the interval timer

            // Handle hour rollover
            if (minutes >= 60)
            {
                minutes -= 60;
                hours++;
            }

            // Handle 24-hour clock rollover
            if (hours >= 24)
            {
                hours = 0; // Reset to midnight
            }
        }

        timerText.text = string.Format("{0:00}:{1:00}", hours, minutes);
    }
    public void ResetTimer()
    {
        elapsedTime = 0f;
        intervalTimer = 0f;
        hours = 12;
        minutes = 0;
        timerText.text = "12:00";
    }

}
