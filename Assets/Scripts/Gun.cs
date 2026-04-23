using System.Collections;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // =====================================================
    // GUN SETTINGS
    // =====================================================
    public float bulletVelocity = 30f;
    public float bulletLifetime = 3f;

    public float fireRate = 0.5f;

    // =====================================================
    // AMMO
    // =====================================================

    public int maxAmmo = 5;
    public int ammoStorage = 15;

    private int currentAmmo;

    // =====================================================
    // TIMERS
    // =====================================================

    public float reloadTime = 3f;
    public float grabTime = 1f;
    public float scavengeTime = 2f;

    // =====================================================
    // STATE
    // =====================================================

    private bool isReloading = false;
    private bool isScavenging = false;

    private float nextTimeToFire;

    // =====================================================
    // REFRENCE
    // =====================================================
    private Animator animator;
    public TextMeshProUGUI ammoText;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    void Awake()
    {
        animator = GetComponent<Animator>();

        currentAmmo = maxAmmo;

        UpdateAmmoUI();
    }

    void Update()
    {
        if (isReloading)
            return;

        HandleShootInput();
        HandleReloadInput();
        HandleGrabInput();
        HandleScavengeInput();
    }

    // =====================================================
    // INPUT
    // =====================================================

    private void HandleShootInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)
            && Time.time >= nextTimeToFire
            && currentAmmo > 0)
        {
            FireWeapon();

            currentAmmo--;

            UpdateAmmoUI();

            nextTimeToFire = Time.time + fireRate;

            // auto reload when empty
            if (currentAmmo <= 0 && ammoStorage > 0)
            {
                StartCoroutine(Reload());
            }
        }
    }

    private void HandleReloadInput()
    {
        if (Input.GetKeyDown(KeyCode.R)
            && currentAmmo < maxAmmo
            && ammoStorage > 0
            && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private void HandleGrabInput()
    {
        if (Input.GetKeyDown(KeyCode.E)
            && Time.time >= nextTimeToFire)
        {
            StartCoroutine(Pickup());

            nextTimeToFire = Time.time + grabTime;
        }
    }

    private void HandleScavengeInput()
    {
        // start holding
        if (Input.GetKeyDown(KeyCode.Mouse1)
            && !isScavenging)
        {
            StartCoroutine(ScavengeAmmo());
        }

        // released early = cancel
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            isScavenging = false;

            animator.SetBool("SCAVENGE", false);
        }
    }

    // =====================================================
    // ACTIONS
    // =====================================================

    private void FireWeapon()
    {
        animator.SetTrigger("RECOIL");

        GameObject bullet = Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            Quaternion.identity
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(
                bulletSpawn.forward * bulletVelocity,
                ForceMode.Impulse
            );
        }

        StartCoroutine(
            DestroyBulletAfterTime(
                bullet,
                bulletLifetime
            )
        );
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        animator.SetTrigger("RELOAD");

        yield return new WaitForSeconds(reloadTime);

        int bulletsNeeded = maxAmmo - currentAmmo;

        if (ammoStorage >= bulletsNeeded)
        {
            currentAmmo += bulletsNeeded;
            ammoStorage -= bulletsNeeded;
        }
        else
        {
            currentAmmo += ammoStorage;
            ammoStorage = 0;
        }

        UpdateAmmoUI();

        isReloading = false;
    }

    private IEnumerator Pickup()
    {
        animator.SetTrigger("GRAB");

        yield return new WaitForSeconds(grabTime);
    }

    private IEnumerator ScavengeAmmo()
    {
        isScavenging = true;

        animator.SetBool("SCAVENGE", true);

        float timer = 0f;

        while (timer < scavengeTime)
        {
            // released early
            if (!isScavenging)
            {
                yield break;
            }

            timer += Time.deltaTime;

            yield return null;
        }

        ammoStorage += 1;

        UpdateAmmoUI();

        animator.SetBool("SCAVENGE", false);

        isScavenging = false;
    }

    private IEnumerator DestroyBulletAfterTime(
        GameObject bullet,
        float time
    )
    {
        yield return new WaitForSeconds(time);

        if (bullet != null)
        {
            Destroy(bullet);
        }
    }

    // =====================================================
    // UI
    // =====================================================

    private void UpdateAmmoUI()
    {
        ammoText.text =
            currentAmmo + "/" + ammoStorage;
    }
}