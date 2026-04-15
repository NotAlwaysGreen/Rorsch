using UnityEngine;

public class ArmLag : MonoBehaviour
{
    public Transform target; // camera
    public float lagAmount = 5f;
    public float smoothSpeed = 10f;

    private Quaternion baseLocalRotation;
    private Quaternion lastRotation;
    private Vector3 rotationVelocity;

    void Start()
    {
        // Store original offset (IMPORTANT)
        baseLocalRotation = transform.localRotation;

        // Store initial camera rotation
        lastRotation = target.rotation;
    }

    void LateUpdate()
    {
        // Rotation change since last frame
        Quaternion delta = target.rotation * Quaternion.Inverse(lastRotation);

        Vector3 deltaEuler = delta.eulerAngles;

        // Fix 0–360 wrap
        if (deltaEuler.x > 180) deltaEuler.x -= 360;
        if (deltaEuler.y > 180) deltaEuler.y -= 360;
        if (deltaEuler.z > 180) deltaEuler.z -= 360;

        // Create lag (invert movement)
        Vector3 targetLag = -deltaEuler * lagAmount;

        // Smooth lag
        rotationVelocity = Vector3.Lerp(rotationVelocity, targetLag, Time.deltaTime * smoothSpeed);

        // ✅ APPLY ON TOP OF BASE ROTATION
        transform.localRotation = baseLocalRotation * Quaternion.Euler(rotationVelocity);

        // Save for next frame
        lastRotation = target.rotation;
    }
}