using UnityEngine;
using TMPro;

public class SF_Shoot : MonoBehaviour
{
    // Stefan Friesen

    public GameObject bullet;
    public int currAmmo, maxAmmo = 7;
    public AudioSource shootAudioSource, reloadAudioSource;

    private GameObject pistole;
    private Rigidbody bulletRB;
    private TMP_Text ammoTextPistole;

    // Start is called before the first frame update
    void Start() {
        ammoTextPistole = gameObject.transform.GetChild(2).GetComponent<TMP_Text>();
        pistole = GetComponent<GameObject>();
        bulletRB = bullet.GetComponent<Rigidbody>();

        ammoTextPistole.text = maxAmmo.ToString();
        currAmmo = maxAmmo;
    }

    // Aktuelle Munitionsanzahl auf Maximum setzen (Nachladen)
    public void Reload() {
        currAmmo = maxAmmo;
        ammoTextPistole.text = currAmmo.ToString();
        reloadAudioSource.Play();
    }

    // Sound eines Schusses abspielen
    public void Sound() {
        shootAudioSource.Play();
    }

    // Kugel aus dem Lauf schießen
    public void Bullet() {
        currAmmo--;
        ammoTextPistole.text = currAmmo.ToString();

        Vector3 pos = this.transform.GetChild(1).transform.position;
        Quaternion rot = this.transform.rotation * Quaternion.Euler(-90.0f, 0, 0);
        Rigidbody spawnedBullet = Instantiate(bulletRB, pos, rot);
        float bulletSpeed = GameObject.Find("VR Rig").GetComponent<SF_Locomotion>().rueckstossKraft + 1736.0f;
        // http://futuretechreport.com/how-fast-does-a-bullet-travel-speed-sniper-mph9mm/
        spawnedBullet.AddForce(transform.forward * bulletSpeed);
    }
}