using System.Diagnostics; // Stopwatch-Type
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SF_StartStopwatch : MonoBehaviour
{
    // Stefan Friesen

    public AudioSource countdown;
    public Stopwatch stopwatch = new Stopwatch();
    public string timeElapsed;
    public bool isCountdownPlayed, stopwatchTrigger;

    // Start is called before the first frame update
    void Start()
    {
        isCountdownPlayed = false;
        stopwatchTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isCountdownPlayed && !stopwatchTrigger && stopwatch.Elapsed.Seconds >= countdown.clip.length) {
            GameObject.Find("Timer").GetComponent<SF_Timer>().timerRend.enabled = true;
            stopwatch.Restart();
            stopwatchTrigger = true;
        }

        timeElapsed = stopwatch.Elapsed.ToString(@"mm\:ss\.ff");
    }

    void OnTriggerEnter()
    {
        if(isCountdownPlayed == false) {
            countdown.Play();
            stopwatch.Start();
            isCountdownPlayed = true;
        }
    }
}
