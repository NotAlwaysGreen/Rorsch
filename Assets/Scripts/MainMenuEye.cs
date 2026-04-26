using UnityEngine;

public class EyeBillboardLook : MonoBehaviour
{
    [Header("References")]
    public Camera cam;

    [Header("Look Settings")]
    public float maxAngle = 10f;   // how far eye can deviate
    public float changeSpeed = 2f;

    private Quaternion targetRotation;
    private float timer;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        PickNewTarget();
    }

    void Update()
    {
        if (cam == null) return;

        // Smoothly rotate toward target
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation * Quaternion.Euler(-90f, 0f, 0f), // model fix
            Time.deltaTime * changeSpeed
        );

        // Timer for new direction
        timer += Time.deltaTime * changeSpeed;

        if (timer >= 1f)
        {
            timer = 0f;
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        // Forward direction to camera
        Vector3 baseDir = cam.transform.position - transform.position;

        // Random rotation cone around that direction (X + Y naturally)
        Vector3 randomDir = Quaternion.Euler(
            Random.Range(-maxAngle, maxAngle),
            Random.Range(-maxAngle, maxAngle),
            0f
        ) * baseDir;

        targetRotation = Quaternion.LookRotation(randomDir, cam.transform.up);
    }
}