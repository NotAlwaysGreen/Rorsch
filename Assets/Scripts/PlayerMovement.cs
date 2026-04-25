using UnityEngine;

public class PlayerMovementm : MonoBehaviour
{
    private InsanityVignette insanityVignette;
    private CharacterController controller;

    public float speed;
    public float ShiftSpeed;
    public float gravity = -9.81f * 2;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("References")]
    [SerializeField] private Stamina stamina;
    [SerializeField] private InsaneBar insaneBar;

    [Header("Punishment")]
    [Range(0f, 1f)]
    [SerializeField] private float insanityIncreaseSpeed = 0.15f;

    private Vector3 velocity;

    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        GameObject vignetteObj = GameObject.FindGameObjectWithTag("Vignette");

        if (vignetteObj != null)
        {
            insanityVignette = vignetteObj.GetComponent<InsanityVignette>();
        }
        // Ground check
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundMask
        );

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isMoving = x != 0 || z != 0;

        bool sprinting =
            Input.GetKey(KeyCode.LeftShift) && isMoving;


        // Movement speed
        float currentSpeed = sprinting ? ShiftSpeed : speed;


        // Move
        Vector3 move = transform.right * x + transform.forward * z;



        controller.Move(
            move * currentSpeed * Time.deltaTime
        );

        // STAMINA
        if (sprinting)
        {
            stamina.Drain();

            // punish if empty
            if (stamina.GetStamina() <= 0f)
            {
                if (insanityVignette != null)
                {
                    insanityVignette.InsaneSprint = true;
                }
                insaneBar.AddInsanity(insanityIncreaseSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (insanityVignette != null)
            {
                insanityVignette.InsaneSprint = false;
            }
            stamina.Regen();
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }
}