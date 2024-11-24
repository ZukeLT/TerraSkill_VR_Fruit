using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public TextMeshPro timerText;
    public bool startTimer = false;
    public float countdownTime = 10f;
    private float currentTime;
    public List<Action> OnTimerEnded = new List<Action>();

    void Start()
    {
        currentTime = countdownTime;
        UpdateTimerText();
    }

    void Update()
    {
        if (startTimer && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            currentTime = Mathf.Max(0, currentTime);

            UpdateTimerText();

            if (currentTime <= 0)
            {
                TimerEnded();
            }
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerEnded()
    {
        startTimer = false;

        foreach (var action in OnTimerEnded)
            action();

        Debug.Log("Timer has ended!");

    }
    public void SetNewTime(float time)
    {
        currentTime = countdownTime = time;
    }
}
