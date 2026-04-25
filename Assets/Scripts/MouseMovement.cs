using UnityEngine;

public class MouseMovement : MonoBehaviour
{
   public float mouseSensitivity = 100f;
    float xRotation = 0f;
    float yRotation = 0f;
    public float topClamp = -90f;
    public float bottomClamp = 90f;

    public float wallTopClamp;
    public float wallBottomClamp;

    //deafukt
    private float defaultTop;
    private float defaultBottom;

    private void Awake()
    {
        defaultTop = topClamp;
        defaultBottom = bottomClamp;
    }


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        esc();
    }

    private static void esc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if collided object is on Wall layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            topClamp = wallTopClamp;
            bottomClamp = wallBottomClamp;

            Debug.Log("Colliding with wall, clamping rotation");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if exited Wall layer collision
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            topClamp = defaultTop;
            bottomClamp = defaultBottom;

            Debug.Log("No longer colliding with wall, unclamping rotation");
        }
    }
}
