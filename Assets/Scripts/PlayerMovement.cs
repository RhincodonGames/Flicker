using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Transform cameraTransform; // drag Main Camera here

    [Header("Movement")]
    public float movementSpeed = 6f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    [Header("Light Gravity / Bob")]
    public float gravity = -2f;       // much lighter than a normal character — you're flying
    public float bobHeight = 0.15f;
    public float bobSpeed = 4f;
    private float bobTimer;
    private Vector3 velocity;

    void Update()
    {
        BasicMovement();
    }

    void BasicMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(horizontal, 0f, vertical);
        inputDir = Vector3.ClampMagnitude(inputDir, 1f);

        // Camera-relative direction — note we do NOT flatten camForward's y,
        // so looking down with the camera actually moves the firefly downward
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        Vector3 moveDir = (camForward * inputDir.z + camRight * inputDir.x);

        if (moveDir.magnitude >= 0.1f)
        {
            // Face the direction of travel (flattened rotation, so the model doesn't pitch weirdly)
            Vector3 flatDir = new Vector3(moveDir.x, 0f, moveDir.z);
            if (flatDir.magnitude > 0.01f)
            {
                float targetAngle = Mathf.Atan2(flatDir.x, flatDir.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                if (!float.IsNaN(angle) && !float.IsInfinity(angle))
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            controller.Move(moveDir.normalized * movementSpeed * Time.deltaTime);
        }

        // Light gravity pull, mostly counteracted so it just feels like a gentle sink instead of true falling
        velocity.y += gravity * Time.deltaTime;
        velocity.y = Mathf.Clamp(velocity.y, gravity * 2f, 0f); // never accumulates into a hard fall

        // Idle/movement bob — subtle up-down while flying, gives that insect hover feel
        bobTimer += Time.deltaTime * bobSpeed * (moveDir.magnitude > 0.1f ? 1f : 0.4f);
        float bobOffset = Mathf.Sin(bobTimer) * bobHeight * Time.deltaTime;

        controller.Move(new Vector3(0, velocity.y * Time.deltaTime + bobOffset, 0));
    }
}