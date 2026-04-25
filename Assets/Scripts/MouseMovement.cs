using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    //[Header("Wall Clamp")]
    //public float wallTopClamp = -50f;
    //public float wallBottomClamp = 50f;

    //[Header("Smooth Transition")]
    //public float clampLerpSpeed = 5f;

    // defaults
    //private float defaultTop;
    //private float defaultBottom;

    //// target clamps
    //private float targetTopClamp;
    //private float targetBottomClamp;

    //private void Awake()
    //{
    //    defaultTop = topClamp;
    //    defaultBottom = bottomClamp;

    //    targetTopClamp = topClamp;
    //    targetBottomClamp = bottomClamp;
    //}

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //// Smoothly transition clamps
        //topClamp = Mathf.Lerp(topClamp, targetTopClamp, clampLerpSpeed * Time.deltaTime);
        //bottomClamp = Mathf.Lerp(bottomClamp, targetBottomClamp, clampLerpSpeed * Time.deltaTime);

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

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
    //    {
    //        targetTopClamp = wallTopClamp;
    //        targetBottomClamp = wallBottomClamp;

            
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
    //    {
    //        targetTopClamp = defaultTop;
    //        targetBottomClamp = defaultBottom;

           
    //    }
    //}
}