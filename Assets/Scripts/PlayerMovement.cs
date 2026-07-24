using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Basic Flying Movement
    public CharacterController controller;
    public Animator animator;

    public float gravity = -9.81f;
    private Vector3 velocity;

    public float movementSpeed = 10f;

    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;

    //private PlayerState playerState;

    private void Start()
    {
        //playerState = PlayerState.Instance;

        turnSmoothVelocity = 0f;
    }

    void Update()
    {
        BasicMovement();
    }

    void BasicMovement()
    {
        // Get Player Input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Calculates movement direction (normalizes so diagonal movement isn't faster)
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Only moves/rotates if players is pressing keys
        if (direction.magnitude >= 0.1f)
        {
            // Calculate angle to face
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Smoothly rotate towards calculated angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // Clamp angle to valid range to prevent quarternion errors
            if (float.IsNaN(angle) || float.IsInfinity(angle))
            {
                angle = targetAngle;
            }
            angle = Mathf.Repeat(angle, 360f);

            // Quaternion (how Unity stores rotations using Euler values) vs. Euler (degrees we can read, X, Y, Z)
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            float currentSpeed = GetCurrentSpeed();

            // Move in that direction
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);

            // Insert Animations Here Later
        }
        else
        {
            // Insert Idle Animation (when not moving)
        }
    }

    float GetCurrentSpeed()
    {
        return movementSpeed;
    }
}
