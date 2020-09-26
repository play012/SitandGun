using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SF_Timer : MonoBehaviour
{
    // Stefan Friesen
    
    public Transform vrCam;
    public MeshRenderer timerRend;
    
    private byte handAppearance;
    private TMP_Text timerText;
    private string timeElapsed;
    private bool stopwatchRunning;

    // Start is called before the first frame update
    void Start()
    {
        timerText = this.GetComponent<TMP_Text>();
        timerRend = this.GetComponent<MeshRenderer>();
        handAppearance = 0;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(vrCam);
        this.transform.Rotate(0, 180.0f, 0, Space.Self);

        stopwatchRunning = GameObject.Find("Startlinie").GetComponent<SF_StartStopwatch>().stopwatchTrigger;
        timeElapsed = GameObject.Find("Startlinie").GetComponent<SF_StartStopwatch>().timeElapsed;
        handAppearance = GameObject.Find("VR Rig").GetComponent<SF_Locomotion>().HandCheck();
        if(stopwatchRunning && handAppearance == 1) {
            timerRend.enabled = true;
            Vector3 rightHand = GameObject.Find("Right Hand Presence").transform.position;
            this.transform.position = new Vector3(rightHand.x, rightHand.y + 0.1f, rightHand.z);
            timerText.text = timeElapsed;
        } else if(stopwatchRunning && handAppearance == 2) {
            timerRend.enabled = true;
            Vector3 leftHand = GameObject.Find("Left Hand Presence").transform.position;
            this.transform.position = new Vector3(leftHand.x, leftHand.y + 0.1f, leftHand.z);
            timerText.text = timeElapsed;
        } else {
            timerRend.enabled = false;
        }
    }
}