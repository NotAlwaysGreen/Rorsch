using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("Movement")]
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float insaneSprintCost = 0.05f;

    [Header("Look")]
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    [Header("Systems")]
    public Stamina staminaSystem;
    public InsaneBar insanitySystem;

    [Header("Insanity")]
    public float insanityGainRate = 0.1f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    public bool canMove = true;

    [Header("References")]
    private CharacterController characterController;
    public InsanityVignette insanityVignette;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

     

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
       
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        bool wantsToRun = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = vertical != 0 || horizontal != 0;

        bool canSprint = staminaSystem != null && staminaSystem.GetStamina() > 0f;

        bool isRunning = wantsToRun && isMoving && canSprint;

        //handel stamina
        if (staminaSystem != null)
        {
            float stamina = staminaSystem.GetStamina();

            // RUNNING
            if (wantsToRun && isMoving && stamina > 0f)
            {
                staminaSystem.Drain();
            }

            // OUT OF STAMINA
            else if (wantsToRun && isMoving && stamina <= 0f)
            {
                if (insanitySystem != null)
                {
                    insanitySystem.AddInsanity(insaneSprintCost * Time.deltaTime);
                }

                if (insanityVignette != null)
                {
                    insanityVignette.SetInsanity(true);
                }
            }

            // NORMAL REGEN
            else
            {
                staminaSystem.Regen();

                if (insanityVignette != null)
                {
                    insanityVignette.SetInsanity(false);
                }
            }
        }

        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        float curSpeedX = canMove ? currentSpeed * vertical : 0;
        float curSpeedY = canMove ? currentSpeed * horizontal : 0;

        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Jump
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Gravity
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    void HandleRotation()
    {
        if (!canMove)
            return;

        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(
            0,
            Input.GetAxis("Mouse X") * lookSpeed,
            0
        );
    }
}