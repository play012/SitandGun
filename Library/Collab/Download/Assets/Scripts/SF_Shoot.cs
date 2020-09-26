using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SF_Shoot : MonoBehaviour
{
    // Stefan Friesen

    public Rigidbody rigidBodyRig; // VR-Rig als Rigidbody

    private AudioSource shootAudioSource; // Audioquelle des Schusses
    private GameObject pistole;

    // Start is called before the first frame update
    void Start() {
        pistole = GameObject.FindWithTag("Pistole"); // Pistole suchen
    }

    // Sound eines Schusses abspielen
    public void Sound() {
        shootAudioSource = GetComponent<AudioSource>();
        shootAudioSource.Play();
    }
}