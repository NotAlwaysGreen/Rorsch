using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Auto References")]
    private Transform player;

    [Header("Interaction")]
    public float interactDistance = 10f;
    public KeyCode interactKey = KeyCode.E;
    public float rayHeight = 2f;

    [Header("Door Settings")]
    public float openAngle = 90f;
    public float openSpeed = 3f;

    private bool isOpen = false;
    private bool isMoving = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        // Auto find player by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player with tag 'Player' not found.");
        }

        // Store door rotations
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0f, 0f, openAngle);
    }

    void Update()
    {
        if (player == null)
            return;

        CheckInteraction();
        RotateDoor();
    }

    void CheckInteraction()
    {
        // Ray starts from player chest/head height
        Vector3 rayOrigin = player.position + Vector3.up * rayHeight;
        Vector3 rayDirection = player.forward;

        Ray ray = new Ray(rayOrigin, rayDirection);
        RaycastHit hit;

        // Debug ray visible in Scene view
        Debug.DrawRay(rayOrigin, rayDirection * interactDistance, Color.red);

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // Check if hit this door or one of its children
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                if (Input.GetKeyDown(interactKey) && !isMoving)
                {
                    ToggleDoor();
                }
            }
        }
    }

    void ToggleDoor()
    {
        isOpen = !isOpen;
        isMoving = true;
    }

    void RotateDoor()
    {
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * openSpeed
        );

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
        {
            transform.rotation = targetRotation;
            isMoving = false;
        }
    }
}