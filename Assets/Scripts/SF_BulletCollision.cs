using UnityEngine;

public class SF_BulletCollision : MonoBehaviour
{
    // Stefan Friesen

    private float objectTimer = 0.0f;

    // Physikbasierter Timer, damit nicht zu viele Projektile wegfliegen
    void FixedUpdate() {
        objectTimer += Time.fixedDeltaTime;
        if(objectTimer > 3.0f) {
            Destroy(gameObject);
            objectTimer = 0;
        }
    }

    // Kollision der Kugel mit Objekten lässt es verschwinden
    void OnCollisionEnter() {
        Destroy(gameObject);
    }
}