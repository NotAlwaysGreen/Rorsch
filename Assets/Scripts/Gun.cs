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

    // =====================================================
    // STATE MACHINE
    // =====================================================

    private enum State
    {
        Idle,
        Reloading,
        ScavengeEnter,
        ScavengeLoop,
        ScavengeExit
    }

    private State state = State.Idle;

    private float nextTimeToFire;

    // =====================================================
    // REFERENCES
    // =====================================================

    private Animator animator;

    public TextMeshProUGUI ammoText;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    // =====================================================
    // UNITY
    // =====================================================

    void Awake()
    {
        animator = GetComponent<Animator>();

        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
        HandleScavengeInput();

        // ONLY reload blocks gameplay
        if (state == State.Reloading)
            return;

        HandleShootInput();
        HandleReloadInput();
        HandleGrabInput();
    }

    void LateUpdate()
    {
        // 🔥 SAFETY NET: prevents permanent scavenge lock
        if (state == State.ScavengeExit)
        {
            var animState = animator.GetCurrentAnimatorStateInfo(0);

            if (!animState.IsName("ExitScavaging"))
            {
                state = State.Idle;
            }
        }
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

            if (currentAmmo <= 0 && ammoStorage > 0)
                StartCoroutine(Reload());
        }
    }

    private void HandleReloadInput()
    {
        if (Input.GetKeyDown(KeyCode.R)
            && currentAmmo < maxAmmo
            && ammoStorage > 0
            && state == State.Idle)
        {
            StartCoroutine(Reload());
        }
    }

    private void HandleGrabInput()
    {
        if (Input.GetKeyDown(KeyCode.E)
            && Time.time >= nextTimeToFire
            && state == State.Idle)
        {
            StartCoroutine(Pickup());
            nextTimeToFire = Time.time + grabTime;
        }
    }

    private void HandleScavengeInput()
    {
        if (state != State.Idle)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            EnterScavenge();
        }
    }

    // =====================================================
    // SCAVENGE FLOW
    // =====================================================

    private void EnterScavenge()
    {
        if (state != State.Idle)
            return;

        state = State.ScavengeEnter;

        animator.ResetTrigger("EXIT_SCAVENGE");
        animator.SetTrigger("ENTER_SCAVENGE");
    }

    private void EnterLoop()
    {
        state = State.ScavengeLoop;
        animator.SetBool("SCAVENGE_HOLD", true);
    }

    private void ExitScavenge()
    {
        if (state == State.ScavengeExit)
            return;

        state = State.ScavengeExit;

        animator.SetBool("SCAVENGE_HOLD", false);

        animator.ResetTrigger("ENTER_SCAVENGE");
        animator.SetTrigger("EXIT_SCAVENGE");
    }

    // =====================================================
    // ANIMATION EVENTS
    // =====================================================

    // EnterScavenge END
    public void OnEnterScavengeFinished()
    {
        if (state != State.ScavengeEnter)
            return;

        if (Input.GetKey(KeyCode.Mouse1))
        {
            EnterLoop();
        }
        else
        {
            ExitScavenge();
        }
    }

    // ExitScavenge END
    public void OnExitScavengeFinished()
    {
        state = State.Idle;
    }

    // Frame 15 scavenging reward
    public void AddScavengeAmmo()
    {
        ammoStorage += 1;
        UpdateAmmoUI();
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
            bulletSpawn.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(bulletSpawn.forward * bulletVelocity, ForceMode.Impulse);
        }

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletLifetime));
    }

    private IEnumerator Reload()
    {
        state = State.Reloading;

        animator.SetTrigger("RELOAD");

        yield return new WaitForSeconds(reloadTime);

        int needed = maxAmmo - currentAmmo;

        if (ammoStorage >= needed)
        {
            currentAmmo += needed;
            ammoStorage -= needed;
        }
        else
        {
            currentAmmo += ammoStorage;
            ammoStorage = 0;
        }

        UpdateAmmoUI();

        state = State.Idle;
    }

    private IEnumerator Pickup()
    {
        animator.SetTrigger("GRAB");
        yield return new WaitForSeconds(grabTime);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        if (bullet) Destroy(bullet);
    }

    // =====================================================
    // UI
    // =====================================================

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo + "/" + ammoStorage;
    }
}