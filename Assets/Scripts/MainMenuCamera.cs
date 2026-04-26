using UnityEngine;

public class MenuCameraOrbit : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Orbit Settings")]
    public float distance = 5f;
    public float horizontalSpeed = 10f;
    public float verticalAmplitude = 2f;
    public float verticalSpeed = 1f;

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

        // Slowly increase angle for horizontal orbit
        angle += Time.deltaTime * horizontalSpeed;

        // Back and forth vertical motion (sine wave)
        float verticalOffset = Mathf.Sin(Time.time * verticalSpeed) * verticalAmplitude;

        // Orbit position
        Vector3 offset = new Vector3(
            Mathf.Sin(angle),
            verticalOffset * 0.1f,
            Mathf.Cos(angle)
        ) * distance;

        Vector3 desiredPosition = target.position + offset;

        // Smooth movement
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            Time.deltaTime * smoothSpeed
        );

        // Always look at target
        transform.LookAt(target);
    }
}