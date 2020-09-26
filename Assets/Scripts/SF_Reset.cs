using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SF_Reset : MonoBehaviour
{
    // Stefan Friesen

    void OnCollisionEnter()
    {
        GameObject.Find("VR Rig").transform.position = new Vector3(16.0f, 0, -20.0f);
        GameObject.Find("VR Rig").transform.rotation = Quaternion.Euler(0, 0, 0);
        GameObject.Find("Pistole").transform.position = new Vector3(15.2f, 0.825f, -17.2f);
        GameObject.Find("Pistole").transform.rotation = Quaternion.Euler(0, 65.0f, 90.0f);
        GameObject.Find("Timer").GetComponent<SF_Timer>().timerRend.enabled = false;
        GameObject.Find("Startlinie").GetComponent<SF_StartStopwatch>().stopwatch.Reset();
        GameObject.Find("Startlinie").GetComponent<SF_StartStopwatch>().isCountdownPlayed = false;
        GameObject.Find("Startlinie").GetComponent<SF_StartStopwatch>().stopwatchTrigger = false;
    }
}
