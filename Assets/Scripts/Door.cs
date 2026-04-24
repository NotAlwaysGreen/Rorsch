using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Auto References")]
    private Transform player;
    private Camera playerCamera;

    [Header("Interaction")]
    public float interactDistance = 3f;
    

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

        // Auto find main camera
        playerCamera = Camera.main;

        // Store rotations
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
    }

    void Update()
    {
        if (player == null || playerCamera == null)
            return;

        CheckInteraction();
        RotateDoor();
    }

    void CheckInteraction()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance > interactDistance)
            return;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                if (Input.GetKeyDown(KeyCode.E) && !isMoving)
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