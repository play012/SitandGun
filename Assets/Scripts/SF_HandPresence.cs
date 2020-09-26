using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SF_HandPresence : MonoBehaviour
{
    // Stefan Friesen

    public InputDeviceCharacteristics controllerCharacteristics;
    public GameObject handModel;
    public bool enableHand { get; set; } = true;
    public GameObject spawnedHand;

    private InputDevice targetDevice;
    private Animator handAnim;

    // Start is called before the first frame update
    void Start() {
        Init();
    }

    // Update is called once per frame
    void Update() {
        if (!targetDevice.isValid) {
            Init();
        } else if (enableHand) {
            UpdateHandAnim();
        } else {    
            Destroy(spawnedHand);
            /* Hand wird hier zerstört,
            damit nicht Hand und Pistole gleichzeitig sichtbar sind */
        }
    }

    // Hände initialisieren
    void Init() {
        List<InputDevice> devices = new List<InputDevice>(); // Liste aus allen VR-Eingabegeräten

        // Inputvariablen in der devices Liste speichern
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        
        if (devices.Count > 0) {
            targetDevice = devices[0];
        }

        if(handModel) {
            spawnedHand = Instantiate(handModel, transform); // Modell der Hand instantiaten
            handAnim = spawnedHand.GetComponent<Animator>(); // Handanimationen zuweisen
        } else {
            Debug.Log("Can't find hands.");
            // Falls das eintrifft sind die Handmodelle verschwunden (gelöscht / verschoben?)
        }
    }

    // Animationen der Hände updaten
    void UpdateHandAnim() {
        if(!spawnedHand) {
            Init();
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue)) {
            handAnim.SetFloat("Trigger", triggerValue);
        } else {
            handAnim.SetFloat("Trigger", 0); // Falls kein Triggerinput verwendet wird
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue)) {
            handAnim.SetFloat("Grip", gripValue);
        } else {
            handAnim.SetFloat("Grip", 0); // Falls kein Gripinput verwendet wird
        }
    }
}
