using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Gemeinsam_Locomotion : MonoBehaviour
{
    // Dieses Skript wurde gemeinsam in mehreren Videoanrufen mit Screensharing geschrieben und bearbeitet

    public float movementSpeed = 1; // Geschwindigkeit mit der man sich mit Stickinput fortbewegt

    private Rigidbody rb; // Rigidbody für den VR-Rig
    private InputDevice targetDeviceR, targetDeviceL; // Zielgerät für rechten & linken Controller
    private XRRig rig; // kompletter VR-Rig (Headset + Controller)
    private GameObject pistole;
    private float rotationTimer;

    // Start is called before the first frame update
    void Start() {
        List<InputDevice> devices = new List<InputDevice>(); // Liste aus allen VR-Eingabegeräten
        rig = GetComponent<XRRig>();
        rb = GetComponent<Rigidbody>();

        // Inputwerte des rechten Controllers als Variable speichern
        InputDeviceCharacteristics controllerCharacteristics = InputDeviceCharacteristics.Controller;
        // Inputvariablen in der devices Liste speichern
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        targetDeviceL = devices[0]; // linker Controller
        targetDeviceR = devices[1]; // rechter Controller
        pistole = GameObject.FindWithTag("Pistole"); // Pistole suchen
    }

    // Update is called once per frame
    void Update() {
        // Werte des rechten Controllers auslesen
        targetDeviceR.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        targetDeviceR.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 inputAxisR);


        // Debugging
        if (triggerValue > 0.1f) {
            Debug.Log(triggerValue);
        }

        // Spieler um 45 Grad rotieren bei Stickinput auf dem rechten Controller
        rotationTimer += Time.deltaTime;
        if(inputAxisR.x > 0.5f && rotationTimer > 0.3f) {
            Vector3 deltaRotationRight = new Vector3(0, 45.0f, 0);
            this.transform.Rotate(deltaRotationRight);
            rotationTimer = 0;
        } else if(inputAxisR.x < -0.5f && rotationTimer > 0.3f) {
            Vector3 deltaRotationLeft = new Vector3(0, -45.0f, 0);
            this.transform.Rotate(deltaRotationLeft);
            rotationTimer = 0;
        } // Rotation kann in Update() bleiben, weil hier nichts kollidiert
    }

    // FixedUpdate für auf Physik basierende Updates
    void FixedUpdate() {
        // Werte des linken Sticks auslesen
        targetDeviceL.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 inputAxisL);

        // Continuous Movement
        Quaternion headDirection = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headDirection * new Vector3(inputAxisL.x, 0, inputAxisL.y);
        rb.AddForce(direction * Time.fixedDeltaTime * movementSpeed);
    }

    // Rueckstoss bei einem Pistolenschuss
    // Aufruf ist im XRGrabInteractable -> Interactable Events -> On Activate
    public void Rueckstoss() {
        float kraft = 1000.0f; // Kraft des Rückstoßes
        Vector3 direction = -pistole.transform.forward * kraft; // Richtung des Rückstoßes
        rb.AddForce(direction * Time.fixedDeltaTime, ForceMode.Impulse); // Impuls in die Richtung
    }
}