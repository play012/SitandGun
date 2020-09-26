using System; // TimeSpan-Type
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SF_StopStopwatch : MonoBehaviour
{
    // Stefan Friesen

    public TimeSpan tsRecent, tsHighscore;

    private bool firstTimeSet;

    // Start is called before the first frame update
    void Start()
    {
        firstTimeSet = false;
    }

    void OnTriggerEnter()
    {
        GameObject.Find("Startlinie").GetComponent<SF_StartStopwatch>().stopwatch.Stop();
        tsRecent = GameObject.Find("Startlinie").GetComponent<SF_StartStopwatch>().stopwatch.Elapsed;

        if (!firstTimeSet) {
            tsHighscore = tsRecent;
            firstTimeSet = true;
        } else if (tsHighscore.CompareTo(tsRecent) > 0) {
            tsHighscore = tsRecent;
        }

        GameObject.Find("ScoreboardScores").GetComponent<TMP_Text>().text = "recent:\t" + tsRecent.ToString(@"mm\:ss\.ff") + "\nbest:\t\t" + tsHighscore.ToString(@"mm\:ss\.ff");
    }
}