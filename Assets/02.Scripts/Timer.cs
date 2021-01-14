using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Fix
public class Timer : MonoBehaviour
{
    public delegate void Timeout();

    private int time;
    public string format;
    public Text timer;
    public Timeout timeout;

    public void StartTimer(int time)
    {
        timer.gameObject.SetActive(true);
        this.time = time;
        InvokeRepeating(nameof(Countdown), 0, 1);        
    }

    public void StopTimer()
    {
        timer.gameObject.SetActive(false);
        CancelInvoke(nameof(Countdown));
    }

    public void Countdown()
    {
        if (time <= 0)
        {
            StopTimer();
            timeout();
        }
        timer.text = string.Format(format, time);

        time--;        
    }
}
