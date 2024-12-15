using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GD;

//learned from Tutorial: https://www.youtube.com/watch?v=POq1i8FyRyQ&t=12s 

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerTextIsometric;
    [SerializeField] TextMeshProUGUI timerTextFirstPerson;
    [SerializeField] GameEvent nightMode;
    [SerializeField] GameEvent dayMode;
    [SerializeField] GameEvent shopOpen;

    // Amount of real seconds needed for 15 minutes of in game time to pass 
    [SerializeField, Range(0.1f, 60f)] float fifteenMinToRealSeconds = 7f;
    //[SerializeField] int secondsUntilNight = 100;
    float elapsedTime = 0f;
    float intervalTimer = 0f;
    int hours = 12; // Start at 12
    int minutes = 0;

    private void Start()
    {
        //StartCoroutine(switchToNight());
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        intervalTimer += Time.deltaTime;

        // Check if desired seconds have passed
        if (intervalTimer >= fifteenMinToRealSeconds)
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

            if (hours == 18 && minutes == 0)
            {
                StartCoroutine(switchToNight());
            }

            if (hours == 8 && minutes == 0)
            {
                StartCoroutine(switchToDay());
            }

            if (hours == 12 && minutes == 0)
            {
                StartCoroutine(openShop());
            }
        }

        timerTextIsometric.text = string.Format("{0:00}:{1:00}", hours, minutes);
        timerTextFirstPerson.text = string.Format("{0:00}:{1:00}", hours, minutes);
    }
    public void ResetTimer()
    {
        elapsedTime = 0f;
        intervalTimer = 0f;
        hours = 12;
        minutes = 0;
        timerTextIsometric.text = "12:00";
        timerTextFirstPerson.text = "12:00";

    }

    private IEnumerator switchToNight()
    {
        yield return new WaitForSeconds(1f);
        nightMode?.Raise();   //raises the event
    }

    private IEnumerator switchToDay()
    {
        yield return new WaitForSeconds(1f);
        dayMode?.Raise();   //raises the event
    }

    private IEnumerator openShop()
    {
        yield return new WaitForSeconds(1f);
        shopOpen?.Raise();   //raises the event
    }

}
