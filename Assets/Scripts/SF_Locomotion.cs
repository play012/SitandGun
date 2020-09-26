using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SF_Locomotion : MonoBehaviour
{
    // Stefan Friesen

    public float movementSpeed = 1; // Geschwindigkeit mit der man sich mit Stickinput fortbewegt
    public float rueckstossKraft = 1000.0f; // Kraft des Rückstoßes

    private SF_Shoot shootScript;
    private Rigidbody rb; // Rigidbody für den VR-Rig
    private InputDevice targetDeviceR, targetDeviceL; // Zielgerät für rechten & linken Controller
    private XRRig rig; // kompletter VR-Rig (Headset + Controller)
    private CapsuleCollider capCollider; // Collider für den Körper
    private GameObject officeChair, pistole, vrCam;
    private float shootTimer;
    private int ammoCount;

    // Start is called before the first frame update
    void Start() {
        List<InputDevice> devices = new List<InputDevice>(); // Liste aus allen VR-Eingabegeräten
        rig = GetComponent<XRRig>();
        rb = GetComponent<Rigidbody>();
        capCollider = GetComponent<CapsuleCollider>();
        officeChair = GameObject.Find("OfficeChair");
        pistole = GameObject.Find("Pistole");
        vrCam = GameObject.Find("VR Camera");
        shootScript = pistole.GetComponent<SF_Shoot>();
        shootTimer = 0.0f;
        ammoCount = 0;

        // Inputwerte des rechten Controllers als Variable speichern
        InputDeviceCharacteristics controllerCharacteristics = InputDeviceCharacteristics.Controller;
        // Inputvariablen in der devices Liste speichern
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        targetDeviceL = devices[0]; // linker Controller
        targetDeviceR = devices[1]; // rechter Controller
    }

    // Update is called once per frame
    void Update() {
        targetDeviceR.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 inputAxisR);

        byte handUpdate = HandCheck();
        float triggerValue = 0.0f;
        if(handUpdate == 1) {
            targetDeviceL.TryGetFeatureValue(CommonUsages.trigger, out triggerValue);
        } else if(handUpdate == 2) {
            targetDeviceR.TryGetFeatureValue(CommonUsages.trigger, out triggerValue);
        }

        // Schusstrigger
        shootTimer += Time.deltaTime;
        ammoCount = shootScript.currAmmo;
        if (triggerValue > 0.9f && shootTimer > 0.3f && ammoCount > 0) {
            shootScript.Sound();
            Rueckstoss();
            shootScript.Bullet();
            shootTimer = 0;
        }

        bool primBtnL, primBtnR, secBtnL, secBtnR;
        targetDeviceL.TryGetFeatureValue(CommonUsages.primaryButton, out primBtnL);
        targetDeviceL.TryGetFeatureValue(CommonUsages.secondaryButton, out primBtnR);
        targetDeviceR.TryGetFeatureValue(CommonUsages.primaryButton, out secBtnL);
        targetDeviceR.TryGetFeatureValue(CommonUsages.secondaryButton, out secBtnR);
        if (primBtnL || primBtnR || secBtnL || secBtnR) {
            shootScript.Reload();
        }
    }

    // FixedUpdate für auf Physik basierende Updates
    void FixedUpdate() {
        // Mitte des Colliders auf die Kamera setzen
        capCollider.center = new Vector3(vrCam.transform.localPosition.x, 1, vrCam.transform.localPosition.z);
        
        // Werte des linken Sticks auslesen
        targetDeviceL.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 inputAxisL);

        // Continuous Movement
        Quaternion headDirection = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 directionMovement = headDirection * new Vector3(inputAxisL.x, 0, inputAxisL.y);
        Vector3 move = directionMovement * Time.fixedDeltaTime * movementSpeed;
        rb.MovePosition(transform.position + move);

        // Bürostuhl verfolgt den Spieler
        officeChair.transform.position = new Vector3(vrCam.transform.position.x, 0, vrCam.transform.position.z);
        officeChair.transform.rotation = headDirection * Quaternion.Euler(0, 180.0f, 0);
    }

    // Rueckstoss bei einem Pistolenschuss
    public void Rueckstoss() {
        Vector3 directionRueckstoss = -this.gameObject.transform.GetChild(0).GetChild(HandCheck()).transform.forward * rueckstossKraft; // Richtung des Rückstoßes
        rb.AddForce(directionRueckstoss * Time.fixedDeltaTime, ForceMode.Impulse); // Impuls in die Richtung
    }

    // Check welche Hand momentan aktiv ist (keine Pistole in der Hand)
    public byte HandCheck() {
        if(!GameObject.Find("Left Hand Presence").GetComponent<SF_HandPresence>().spawnedHand) {
            return 1;
        } else if (!GameObject.Find("Right Hand Presence").GetComponent<SF_HandPresence>().spawnedHand) {
            return 2;
        }
        return 0;
    }
}