using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Distance")]
    public float baseDistance = 8f;
    public float zoomPerFriend = 0.8f;
    public float maxDistance = 18f;
    private float currentDistance;
    public float zoomSmoothSpeed = 3f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 3f;
    public float minPitch = -20f;
    public float maxPitch = 60f;
    private float yaw = 0f;
    private float pitch = 15f;

    [Header("Clipping Prevention")]
    public LayerMask collisionMask;
    public float cameraRadius = 0.3f;

    private void Start()
    {
        currentDistance = baseDistance;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (target == null) return;
        if (PauseMenuManager.IsPaused) return;

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        float friends = GameManager.Instance != null ? GameManager.Instance.fireflyFriendsCollected : 0;
        float targetDistance = Mathf.Clamp(baseDistance + friends * zoomPerFriend, baseDistance, maxDistance);
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, zoomSmoothSpeed * Time.deltaTime);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredOffset = rotation * new Vector3(0f, 0f, -currentDistance);

        Vector3 desiredCameraPos = target.position + desiredOffset;
        Vector3 directionToCamera = desiredCameraPos - target.position;
        float distanceToCamera = directionToCamera.magnitude;

        if (Physics.SphereCast(target.position, cameraRadius, directionToCamera.normalized, out RaycastHit hit, distanceToCamera, collisionMask))
        {
            transform.position = target.position + directionToCamera.normalized * (hit.distance - 0.1f);
        }
        else
        {
            transform.position = desiredCameraPos;
        }

        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }
}