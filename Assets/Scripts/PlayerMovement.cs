using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Transform cameraTransform;

    [Header("Horizontal Movement")]
    public float movementSpeed = 6f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    [Header("Vertical Movement (Space = up, release = sink)")]
    public float ascendSpeed = 4f;
    public float gravity = -9.81f;
    public float groundedStickForce = -2f;
    private Vector3 verticalVelocity;

    [Header("Dash (Ctrl)")]
    public float dashSpeed = 18f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1.5f;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private Vector3 dashDirection;

    [Header("Bob")]
    public float bobHeight = 0.15f;
    public float bobSpeed = 4f;
    private float bobTimer;

    void Update()
    {
        HandleDashTimers();
        Vector3 moveDir = GetCameraRelativeInputDirection();
        HandleRotationAndMovement(moveDir);
        HandleVerticalMovement();
    }

    Vector3 GetCameraRelativeInputDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(horizontal, 0f, vertical);
        inputDir = Vector3.ClampMagnitude(inputDir, 1f);

        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f; // keep horizontal movement flat — vertical is handled separately by Space
        camForward.Normalize();
        Vector3 camRight = cameraTransform.right;
        camRight.y = 0f;
        camRight.Normalize();

        return camForward * inputDir.z + camRight * inputDir.x;
    }

    void HandleRotationAndMovement(Vector3 moveDir)
    {
        // Dashing overrides normal movement for its short duration
        if (dashTimer > 0f)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            dashTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && dashCooldownTimer <= 0f)
        {
            // Dash in current movement direction, or facing direction if standing still
            dashDirection = moveDir.magnitude >= 0.1f ? moveDir.normalized : transform.forward;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
            return;
        }

        if (moveDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            if (!float.IsNaN(angle) && !float.IsInfinity(angle))
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(moveDir.normalized * movementSpeed * Time.deltaTime);

            bobTimer += Time.deltaTime * bobSpeed;
            float bobOffset = Mathf.Sin(bobTimer) * bobHeight * Time.deltaTime;
            controller.Move(new Vector3(0, bobOffset, 0));
        }
    }

    void HandleVerticalMovement()
    {
        bool ascending = Input.GetKey(KeyCode.Space);

        if (ascending)
            verticalVelocity.y = ascendSpeed;
        else if (controller.isGrounded)
            verticalVelocity.y = groundedStickForce;
        else
            verticalVelocity.y += gravity * Time.deltaTime;

        controller.Move(verticalVelocity * Time.deltaTime);
    }

    void HandleDashTimers()
    {
        if (dashCooldownTimer > 0f) dashCooldownTimer -= Time.deltaTime;
    }

    public bool IsDashing => dashTimer > 0f;
    public float DashCooldownRemaining => Mathf.Max(0f, dashCooldownTimer);
}