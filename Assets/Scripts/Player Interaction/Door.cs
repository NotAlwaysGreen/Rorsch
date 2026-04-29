using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = 90f;
    public float openSpeed = 3f;

    [Header("Lock Settings")]
    public bool isLocked = false; 
    

    private bool isOpen = false;
    private bool playerInRange = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0f, 0f, openAngle);
    }

    void Update()
{
    if (playerInRange && Input.GetKeyDown(KeyCode.Space))
    {
        if (isLocked)
        {
            UIManager.Instance.ShowMessage("This door is locked");
            return;
        }

        
        ToggleDoor();
    }

    RotateDoor();
}

    void ToggleDoor()
    {
        isOpen = !isOpen;
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
        }
    }

    public void Unlock()
    {
        isLocked = false;
        Debug.Log("Door unlocked!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}