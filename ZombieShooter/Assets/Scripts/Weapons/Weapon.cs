using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Weapon : MonoBehaviour
{
    public int maxAmmoAmount { get => _maxAmmoAmount; }
    public bool isReloading { get; private set; }
    [Range(0, 30)] public int spreadAngle;
    [HideInInspector] public int currentAmmoAmount { get; private set; }

    [SerializeField] private int _maxAmmoAmount;
    [SerializeField] private float shotTimeBetween;
    [SerializeField] private float reloadTime;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject shotEffect;

    [Space(20)]
    [SerializeField] private Transform[] spawnPoints;

    [Header("PICKUP")]
    [SerializeField] private bool isEquipped;
    [SerializeField] private SpriteRenderer weapon, weaponItem;

    [Header ("AUDIO")]
    [SerializeField] private AudioSource shotAudio;
    [SerializeField] private AudioSource reloadAudio;

    private CircleCollider2D collider;
    private float shotTimeLeft;
    private int spawnPointIndex;

    private void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
        currentAmmoAmount = Random.Range(0, maxAmmoAmount);

        if (isEquipped)
        {
            weapon.enabled = true;
            weaponItem.enabled = false;
            collider.enabled = false;
        }
        else
        {
            weapon.enabled = false;
            weaponItem.enabled = true;
            collider.enabled = true;
        }
    }

    private void Update()
    {
        if (shotTimeLeft > 0)
            shotTimeLeft -= Time.deltaTime;
    }

    public void Reload(int ammo)
    {
        if(!isReloading)
            StartCoroutine(StartReloading(ammo));
    }

    IEnumerator StartReloading(int ammo)
    {
        isReloading = true;
        reloadAudio.Play();

        yield return new WaitForSeconds(reloadTime);

        currentAmmoAmount += ammo;
        reloadAudio.Stop();
        isReloading = false;
    }

    public void TryShoot()
    {
        if (shotTimeLeft <= 0 && currentAmmoAmount > 0)
        {
            Transform spawnPoint = spawnPoints[spawnPointIndex];
            Instantiate(bulletPrefab, spawnPoint.position, GetSpreadRotation(spawnPoint.rotation));
            Instantiate(shotEffect, spawnPoint.position, spawnPoint.rotation);

            shotAudio.PlayOneShot(shotAudio.clip);
            shotTimeLeft = shotTimeBetween;
            currentAmmoAmount--;
            spawnPointIndex++;

            if (spawnPointIndex >= spawnPoints.Length)
                spawnPointIndex = 0;
        }
    }

    private Quaternion GetSpreadRotation(Quaternion currentRotation)
    {
        int lowOffset = 0 - spreadAngle / 2;
        int highOffset = spreadAngle / 2;
        int rotationZ = Random.Range(lowOffset, highOffset);

        currentRotation = Quaternion.Euler(0, 0, currentRotation.eulerAngles.z + rotationZ);

        return currentRotation;
    }

    public void Pickup()
    {
        weapon.enabled = true;
        weaponItem.enabled = false;
        collider.enabled = false;
    }

    public void Drop()
    {
        weapon.enabled = false;
        weaponItem.enabled = true;
        collider.enabled = true;
        transform.parent = null;

        reloadAudio.Stop();
    }
}
