using UnityEngine;

public class MenuCameraOrbit : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Orbit Settings")]
    public float distance = 5f;
    public float horizontalSpeed = 10f;

    [Range(-89f, 89f)]
    public float verticalAngle = 20f; // fixed angle in degrees

    [Header("Smoothing")]
    public float smoothSpeed = 2f;

    private float angle;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("No target assigned for camera orbit.");
            enabled = false;
        }
    }

    void Update()
    {
        if (target == null) return;

        // Horizontal rotation
        angle += Time.deltaTime * horizontalSpeed;

        // Convert angles to radians
        float radH = angle * Mathf.Deg2Rad;
        float radV = verticalAngle * Mathf.Deg2Rad;

        // Spherical to Cartesian conversion
        float x = Mathf.Cos(radV) * Mathf.Sin(radH);
        float y = Mathf.Sin(radV);
        float z = Mathf.Cos(radV) * Mathf.Cos(radH);

        Vector3 offset = new Vector3(x, y, z) * distance;
        Vector3 desiredPosition = target.position + offset;

        // Smooth movement
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            Time.deltaTime * smoothSpeed
        );

        transform.LookAt(target);
    }
}