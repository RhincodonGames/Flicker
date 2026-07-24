using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Player
    public Transform target;

    // Camera pos. rel. to player
    public Vector3 offset = new Vector3(0f, 10f, -8f);

    public float smoothSpeed = 5f;  // Higher = snappier, lower = smoother
    public bool smoothFollow = true;

    private Quaternion initialRotation;

    private void Start()
    {
        // Store manual rotation setting in Inspector
        initialRotation = transform.rotation;

        // Initialize camera pos. on scene load to prevent zoom issues
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
    // Runs after Update() so camera movement is smoother
    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        if (smoothFollow)
        {
            // Smooth follow function
            // Lerp = linear interpolation (smoothing bewteen two values)
            // Smooths over time between current position and desired position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
        else
        {
            // Instant follow 
            transform.position = desiredPosition;
        }

        // Keep set rotation angle
        transform.rotation = initialRotation;
    }
}