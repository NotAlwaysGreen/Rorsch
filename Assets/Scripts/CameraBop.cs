using UnityEngine;

public class CameraBob : MonoBehaviour
{
    [Header("Walk")]
    public float walkBobSpeed = 4f;
    public float walkBobAmount = 0.03f;

    [Header("Run")]
    public float runBobSpeed = 7f;
    public float runBobAmount = 0.05f;

    [Header("Smooth")]
    public float smoothSpeed = 8f;

    private Vector3 startPos;
    private float timer;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        bool isMoving = x != 0 || z != 0;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        if (isMoving)
        {
            float speed = isRunning ? runBobSpeed : walkBobSpeed;
            float amount = isRunning ? runBobAmount : walkBobAmount;

            timer += Time.deltaTime * speed;

            
            float y = Mathf.Sin(timer) * amount;

            Vector3 targetPos = new Vector3(
                startPos.x,
                startPos.y + y,
                startPos.z
            );

            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                targetPos,
                Time.deltaTime * smoothSpeed
            );
        }
        else
        {
            timer = 0;

            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                startPos,
                Time.deltaTime * smoothSpeed
            );
        }
    }
}