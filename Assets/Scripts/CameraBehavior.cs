using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Orbit Distance (zoom out as fireflies are collected)")]
    public float baseDistance = 8f;
    public float zoomPerFriend = 0.8f;   // how much further out per firefly friend collected
    public float maxDistance = 18f;
    public float height = 3f;            // base vertical offset, similar role to your old offset.y

    [Header("Mouse Look")]
    public float mouseSensitivity = 3f;
    public float minPitch = -20f;
    public float maxPitch = 60f;

    [Header("Smoothing")]
    public float smoothSpeed = 5f;
    public bool smoothFollow = true;

    private float yaw = 0f;
    private float pitch = 15f;

    private void Start()
    {
        if (target != null)
        {
            transform.position = target.position + new Vector3(0, height, -baseDistance);
        }
        Cursor.lockState = CursorLockMode.Locked; // hide/lock cursor since mouse now controls the camera
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Mouse input drives orbit angles
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Distance grows with collected friends (read from GameManager)
        float friends = GameManager.Instance != null ? GameManager.Instance.fireflyFriendsCollected : 0;
        float distance = Mathf.Clamp(baseDistance + friends * zoomPerFriend, baseDistance, maxDistance);

        // Compute orbit position: rotate an offset behind the target by yaw/pitch
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredOffset = rotation * new Vector3(0f, 0f, -distance);
        Vector3 desiredPosition = target.position + desiredOffset + Vector3.up * height * 0.3f;

        if (smoothFollow)
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        else
            transform.position = desiredPosition;

        // Always look at the target (this replaces your locked initialRotation)
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }
}