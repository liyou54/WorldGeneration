using System;
using UnityEngine;

public class PerformanceTimer : IDisposable
{
    private string timerName;
    private DateTime startTime;

    public PerformanceTimer(string name)
    {
        timerName = name;
        StartTimer();
    }

    public void StartTimer()
    {
        startTime = DateTime.Now;
    }

    public void StopTimer()
    {
        TimeSpan elapsedTime = DateTime.Now - startTime;
        Debug.Log($"{timerName} executed in {elapsedTime.TotalMilliseconds} milliseconds");
    }

    public void Dispose()
    {
        StopTimer();
    }
}