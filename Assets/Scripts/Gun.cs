
using System.Collections;
using TMPro;
using Unity.Hierarchy;
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
    public float reduceInsanityAmount = 0.2f;
    public float scavengeTime = 2f;

    // =====================================================
    // STATE
    // =====================================================

    private bool isReloading = false;
    private bool isScavenging = false;
    private bool isPickingUp = false;
   

    private float nextTimeToFire;

    private Coroutine scavengeRoutine;

    // =====================================================
    // REFERENCES
    // =====================================================

    private Animator animator;

    public TextMeshProUGUI ammoText;

    public GameObject bulletPrefab;

    public Transform bulletSpawn;
    public InsaneBar insaneBar;
    private GameObject currentPill;
    void Awake()
    {
        animator = GetComponent<Animator>();

        currentAmmo = maxAmmo;

        UpdateAmmoUI();
    }

    void Update()
    {
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
            && currentAmmo > 0
            && !isReloading
            && !isScavenging)
        {
            FireWeapon();

            currentAmmo--;

            UpdateAmmoUI();

            nextTimeToFire = Time.time + fireRate;

            // auto reload
            if (currentAmmo <= 0
                && ammoStorage > 0)
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
            && !isReloading
            && !isScavenging)
        {
            StartCoroutine(Reload());
        }
    }

    private void HandleGrabInput()
    {
        if (Input.GetKeyDown(KeyCode.Space)
            && Time.time >= nextTimeToFire
            && !isReloading
            && !isScavenging)
        {
            StartCoroutine(Pickup());

            nextTimeToFire = Time.time + grabTime;
        }
    }

    private void HandleScavengeInput()
    {
        // start scavenging
        if (Input.GetKeyDown(KeyCode.Mouse1)
            && !isScavenging
            && !isReloading)
        {
            scavengeRoutine =
                StartCoroutine(ScavengeAmmo());
        }

        // stop scavenging
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            StopScavenging();
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

        Rigidbody rb =
            bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(
                bulletSpawn.forward
                * bulletVelocity,
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

        // interrupt scavenging
        StopScavenging();

        animator.SetTrigger("RELOAD");

        yield return new WaitForSeconds(
            reloadTime
        );

        int bulletsNeeded =
            maxAmmo - currentAmmo;

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

        if (isPickingUp && currentPill != null)
        {
            insaneBar.ReduceInsanity(reduceInsanityAmount);

            Destroy(currentPill);

            currentPill = null;
            isPickingUp = false;
        }

        yield return new WaitForSeconds(grabTime);
    }
    //for grab
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pills"))
        {
            
            isPickingUp = true;
            currentPill = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pills"))
        {
            

            isPickingUp = false;
            currentPill = null;
        }
    }
    //end
    private IEnumerator ScavengeAmmo()
    {
        isScavenging = true;

        animator.SetBool(
            "IS_SCAVAGING",
            true
        );

        while (isScavenging)
        {
            float timer = 0f;

            while (timer < scavengeTime)
            {
                if (!isScavenging)
                {
                    animator.SetBool(
                        "IS_SCAVAGING",
                        false
                    );

                    yield break;
                }

                timer += Time.deltaTime;

                yield return null;
            }

            

            
        }

        animator.SetBool(
            "IS_SCAVAGING",
            false
        );
    }

    private void StopScavenging()
    {
        isScavenging = false;

        animator.SetBool(
            "IS_SCAVAGING",
            false
        );

        if (scavengeRoutine != null)
        {
            StopCoroutine(
                scavengeRoutine
            );

            scavengeRoutine = null;
        }
    }

    private IEnumerator DestroyBulletAfterTime(
        GameObject bullet,
        float time
    )
    {
        yield return new WaitForSeconds(
            time
        );

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
            currentAmmo
            + "/"
            + ammoStorage;
    }

    public void AddAmmo()
    {
        ammoStorage += 1;

        UpdateAmmoUI();
    }
}

