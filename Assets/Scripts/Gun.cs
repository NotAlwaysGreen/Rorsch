using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifetime = 3f;

    public float fireRate = 0.5f;
    private float nextTimeToFire = 0f;

    public int maxAmmo = 5;
    private int currentAmmo;

    public float reloadTime = 3f;
    public float grabTime = 1f;
    private bool isReloading = false;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        currentAmmo = maxAmmo;
    }

    void Update()
{
    if (isReloading)
        return;

    // SHOOTING
    if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextTimeToFire)
    {
        if (currentAmmo > 1)
        {
            FireWeapon();
            currentAmmo--;
            nextTimeToFire = Time.time + fireRate;
        }
        else
        {
            FireWeapon();
            currentAmmo--;
            nextTimeToFire = Time.time + fireRate;
            StartCoroutine(Reload());
        }

        return; // important: prevents grab on same frame
    }

    // GRAB (only when idle)
    if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextTimeToFire)
    {
        StartCoroutine(Pickup());
        nextTimeToFire = Time.time + grabTime; // prevent shooting immediately after grab
    }
}

    private IEnumerator Pickup()
    {   
        animator.SetTrigger("GRAB");
        yield return new WaitForSeconds(grabTime);
    }
    private void FireWeapon()
    {
        animator.SetTrigger("RECOIL");

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifetime));
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        animator.SetTrigger("RELOAD");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float bulletPrefabLifetime)
    {
        yield return new WaitForSeconds(bulletPrefabLifetime);
        Destroy(bullet);
    }
}