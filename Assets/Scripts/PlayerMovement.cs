using UnityEngine;

public class PlayerMovementm : MonoBehaviour
{
    private CharacterController controller;

    public float speed;
    public float ShiftSpeed;
    public float gravity = -9.81f * 2;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;

    private bool isGrounded;

    private Vector3 lastPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        lastPosition = transform.position;
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Hold Shift to run
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? ShiftSpeed : speed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        lastPosition = transform.position;
    }
}